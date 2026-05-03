using Microsoft.EntityFrameworkCore;
using LogStreamX.Infrastructure.Models;

namespace LogStreamX.Infrastructure.Data;

public class LogDbContext : DbContext
{
    public LogDbContext(DbContextOptions<LogDbContext> options)
        : base(options)
    {
    }

    // ✅ SINGLE CONSISTENT NAME (THIS FIXES YOUR ERROR)
    public DbSet<LogEntry> Logs { get; set; }
}