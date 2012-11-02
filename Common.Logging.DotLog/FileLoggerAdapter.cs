// #region License
// ========================================================================
// FileLoggerAdapter.cs
// dotlog - Simple Logging for .NET
// https://github.com/tonypujals/dotlog
// 
// Copyright (C) 2012, Tony Pujals
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ========================================================================
// #endregion

namespace Common.Logging.DotLog
{
	using System;
	using System.Collections.Specialized;
	using System.Linq;
	using System.Text.RegularExpressions;
	using Factory;
	using global::DotLog;

	internal static class EnumHelper
	{
		public static bool In<T>(this T value, params T[] list)
		{
			return list.Contains(value);
		}
	}

	public class FileLoggerAdapter : AbstractLogger
	{
		private const string FileNameTemplateKey = "fileNameTemplate";
		private const string LogPathKey = "logPath";
		private const string LogLevelKey = "logLevel";
		private const string ShowDateTimeKey = "showDateTime";
		private const string DateTimeFormatKey = "dateTimeFormat";
		private const string UseUtcKey = "useUTC";
		private const string DefaultCategoryKey = "defaultCategoryKey";

		private const string CategoryPattern = @"^(\s*\|\s*(?<category>[^\|]*)\s*\|\s*)?(?<message>.*)";

		private readonly FileLogger _fileLogger;
		private readonly Regex _regex;

		internal FileLoggerAdapter(NameValueCollection properties)
		{
			_regex = new Regex(CategoryPattern, RegexOptions.None);

			var fileNameTemplate = properties.Get(FileNameTemplateKey);
			var logPath = properties.Get(LogPathKey);
			var logLevelStr = properties.Get(LogLevelKey);
			var showDateTime = properties.Get(ShowDateTimeKey);
			var dateTimeFormat = properties.Get(DateTimeFormatKey);
			var useUtc = properties.Get(UseUtcKey);
			var defaultCategory = properties.Get(DefaultCategoryKey);

			var logLevel = LogLevel.Verbose;
			if (!string.IsNullOrWhiteSpace(logLevelStr))
			{
				switch (logLevelStr.ToLowerInvariant())
				{
					case "all":
					case "trace":
					case "debug":
						logLevel = LogLevel.Verbose;
						break;

					case "info":
						logLevel = LogLevel.Information;
						break;

					case "warn":
						logLevel = LogLevel.Warning;
						break;

					case "error":
						logLevel = LogLevel.Error;
						break;

					case "fatal":
						logLevel = LogLevel.Critical;
						break;
				}
			}

			_fileLogger = new FileLogger
			{
				FileNameTemplate = fileNameTemplate,
				LogPath = logPath,
				LogLevel = logLevel,
				// todo - support other configuration properties
			};
		}

		public override bool IsTraceEnabled
		{
			get
			{
				return _fileLogger.LogLevel.In(
					LogLevel.Verbose);
			}
		}

		public override bool IsDebugEnabled
		{
			get
			{
				return _fileLogger.LogLevel.In(
					LogLevel.Verbose);
			}
		}

		public override bool IsInfoEnabled
		{
			get
			{
				return _fileLogger.LogLevel.In(
					LogLevel.Verbose,
					LogLevel.Information);
			}
		}

		public override bool IsWarnEnabled
		{
			get
			{
				return _fileLogger.LogLevel.In(
					LogLevel.Verbose,
					LogLevel.Information,
					LogLevel.Warning);
			}
		}

		public override bool IsErrorEnabled
		{
			get
			{
				return _fileLogger.LogLevel.In(
					LogLevel.Verbose,
					LogLevel.Information,
					LogLevel.Warning,
					LogLevel.Error);
			}
		}

		public override bool IsFatalEnabled
		{
			get
			{
				return _fileLogger.LogLevel.In(
					LogLevel.Verbose,
					LogLevel.Information,
					LogLevel.Warning,
					LogLevel.Error,
					LogLevel.Critical);
			}
		}

		protected override void WriteInternal(global::Common.Logging.LogLevel level, object message, Exception exception)
		{
			var logLevel = LogLevel.Verbose;

			switch (level)
			{
				case global::Common.Logging.LogLevel.All:
				case global::Common.Logging.LogLevel.Trace:
				case global::Common.Logging.LogLevel.Debug:
					logLevel = LogLevel.Verbose;
					break;

				case global::Common.Logging.LogLevel.Info:
					logLevel = LogLevel.Information;
					break;

				case global::Common.Logging.LogLevel.Warn:
					logLevel = LogLevel.Warning;
					break;

				case global::Common.Logging.LogLevel.Error:
					logLevel = LogLevel.Error;
					break;

				case global::Common.Logging.LogLevel.Fatal:
					logLevel = LogLevel.Critical;
					break;
			}


			string category = null;
			string content = null;

			// since Common.Logging doesn't have support for categories, we provide support
			// for parsing out a category from the message itself. For example:
			// "|system| blah blah blah..."
			// ==> category: system
			// ==> message: blah blah blah
			var match = _regex.Match(message.ToString());
			if (match.Success)
			{
				category = match.Groups["category"].Value;
				content = match.Groups["message"].Value;
			}

			var logMessage = string.Format("{0}{1}{2}",
				(content ?? string.Empty),
				(string.IsNullOrEmpty(content) ? " " : string.Empty),
				(exception != null ? "EXCEPTION: " + exception : string.Empty)
				);

			_fileLogger.Log(logMessage, logLevel, category);
		}
	}
}