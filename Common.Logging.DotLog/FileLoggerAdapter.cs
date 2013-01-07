// #region License
// ========================================================================
// FileLoggerAdapter.cs
// DotLog - Simple Logging for .NET
// https://github.com/tonypujals/dotlog
// 
// Copyright © 2012, 2013 Tony Pujals and Subfuzion, Inc.
// 
// The MIT License (MIT)
// 
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be included
// in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
// CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
// SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
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