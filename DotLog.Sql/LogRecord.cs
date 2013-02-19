namespace DotLog.Sql
{
	using System;

	public class LogRecord : LogEntry
	{
		public Guid ID { get; set; }

		/// <summary>
		/// This is the LogLevel as a string value.
		/// Entity Framework won't map types it doesn't support, and it doesn't
		/// currently support enums, so save the enum as a string
		/// </summary>
		public string LogLevelName { get; set; }

		/// <summary>
		/// This is the LogLevel as a string value.
		/// Entity Framework won't map types it doesn't support, and it doesn't
		/// currently support enums, so save the enum as a string
		/// </summary>
		public int LogLevelValue { get; set; }
	}
}