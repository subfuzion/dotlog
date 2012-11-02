// #region License
// ========================================================================
// Logger.cs
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

namespace DotLog
{
	using System;

	/// <summary>
	///   An instance of this logger will write the formatted output
	///   of the Log method to the supplied log action.
	/// </summary>
	public class Logger : ILogger
	{
		public const LogLevel DefaultLogLevel = LogLevel.Verbose;

		public Logger(LogLevel logLevel = LogLevel.Verbose)
		{
			LogLevel = logLevel;
		}

		public Logger(Action<string> logAction, LogLevel logLevel = DefaultLogLevel)
			: this(logLevel)
		{
			LogAction = logAction;
		}

		/// <summary>
		///   Gets or sets the log action to use to write the formatted output
		///   after the Log method is invoked.
		/// </summary>
		public Action<string> LogAction { get; set; }

		/// <summary>
		///   Gets or sets the formatter for formatting the log entry.
		/// </summary>
		public ILogFormatter Formatter { get; set; }

		/// <summary>
		///   Gets or sets the current log level.
		///   This is used to determine which log entries are written to the log destination.
		/// </summary>
		public LogLevel LogLevel { get; set; }

		#region ILogger Members

		/// <summary>
		///   Write the log entry to the output destination.
		/// </summary>
		/// <param name="message"> Must not be null or empty </param>
		/// <param name="logLevel"> </param>
		/// <param name="category"> </param>
		/// <returns> </returns>
		public virtual LogEntry Log(string message, LogLevel logLevel = LogLevel.Verbose, string category = null)
		{
			if (string.IsNullOrWhiteSpace(message))
			{
				throw new ArgumentNullException("message");
			}

			var logEntry = new LogEntry
			{
				Timestamp = DateTime.UtcNow,
				Message = message,
				LogLevel = logLevel,
				Category = category,
			};

			if (LogAction != null && logLevel != LogLevel.None && logLevel <= LogLevel)
			{
				// if Formatter is null, generates a default formatted string
				var output = logEntry.ToString(Formatter);

				LogAction(output);
			}

			return logEntry;
		}

		#endregion
	}
}