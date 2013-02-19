namespace DotLog.Sql
{
	using System;

	public class SqlLogger : ILogger
	{
		public LogEntry Log(string message, LogLevel logLevel = LogLevel.Verbose, string category = null)
		{
			if (string.IsNullOrWhiteSpace(message))
			{
				throw new ArgumentNullException("message");
			}

			var logRecord = new LogRecord
			{
				ID = Guid.NewGuid(),
				Timestamp = DateTime.UtcNow,
				Message = message,
				LogLevelName = logLevel.ToString(),
				LogLevelValue = (int)logLevel,
				Category = category,
			};

			if (logLevel != LogLevel.None && logLevel >= LogLevel)
			{
				using (var db = new LogContext())
				{
					db.LogRecords.Add(logRecord);
					db.SaveChangesSync();
				}
			}

			return logRecord;
		}

		/// <summary>
		///   Gets or sets the current log level.
		///   This is used to determine which log entries are written to the log destination.
		/// </summary>
		public LogLevel LogLevel { get; set; }
	}
}