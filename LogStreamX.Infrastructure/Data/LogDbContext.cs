using Microsoft.EntityFrameworkCore;
using LogStreamX.Infrastructure.Models;

namespace LogStreamX.Infrastructure.Data;

public class LogDbContext : DbContext
{
    public LogDbContext(DbContextOptions<LogDbContext> options) : base(options)
    {
    }

    // ✅ STANDARD NAME (USE THIS EVERYWHERE)
    public DbSet<LogEntry> Logs { get; set; }
}