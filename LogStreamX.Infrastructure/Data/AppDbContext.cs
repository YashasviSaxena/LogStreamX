using LogStreamX.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace LogStreamX.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<LogEntry> LogEntries { get; set; }
    }
}