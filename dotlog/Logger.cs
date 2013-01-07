// #region License
// ========================================================================
// Logger.cs
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

			if (LogAction != null && logLevel != LogLevel.None && logLevel >= LogLevel)
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