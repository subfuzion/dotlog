namespace DotLog.Sql
{
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Data.Entity;

	public class LogContext : DbContext
	{
		private readonly object _mutex = new object();

		public DbSet<LogRecord> LogRecords { get; set; }

		public LogContext()
			: base("DotLog")
		{
		}

		public void SaveChangesSync()
		{
			lock (_mutex)
			{
				SaveChanges();
			}
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			var logRecord = modelBuilder.Entity<LogRecord>();
			logRecord.Property(e => e.ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
		}
	}
}
