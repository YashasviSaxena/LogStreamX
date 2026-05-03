using Microsoft.EntityFrameworkCore;
using LogStreamX.Infrastructure.Models;

namespace LogStreamX.Infrastructure.Data
{
    public class LogDbContext : DbContext
    {
        public LogDbContext(DbContextOptions<LogDbContext> options) : base(options) { }

        public DbSet<LogEntry> LogEntries => Set<LogEntry>();
    }
}