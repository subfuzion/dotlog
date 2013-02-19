namespace DotLog
{
	public static class LogExtensions
	{
		public static LogEntry LogVerbose(this ILogger logger, string message, string category = null)
		{
			return logger.Log(message, LogLevel.Verbose, category);
		}

		public static LogEntry LogInformation(this ILogger logger, string message, string category = null)
		{
			return logger.Log(message, LogLevel.Information, category);
		}

		public static LogEntry LogWarning(this ILogger logger, string message, string category = null)
		{
			return logger.Log(message, LogLevel.Warning, category);
		}

		public static LogEntry LogError(this ILogger logger, string message, string category = null)
		{
			return logger.Log(message, LogLevel.Error, category);
		}

		public static LogEntry LogCritical(this ILogger logger, string message, string category = null)
		{
			return logger.Log(message, LogLevel.Critical, category);
		}
	}
}