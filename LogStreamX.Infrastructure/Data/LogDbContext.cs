using Microsoft.EntityFrameworkCore;

namespace LogStreamX.Infrastructure.Data
{
    public class LogDbContext : DbContext
    {
        public LogDbContext(DbContextOptions<LogDbContext> options)
            : base(options)
        {
        }

        public DbSet<LogEntry> LogEntries { get; set; }
    }
}