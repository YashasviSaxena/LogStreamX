using LogStreamX.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace LogStreamX.Infrastructure.Data;

public class LogDbContext : DbContext
{
    public LogDbContext(DbContextOptions<LogDbContext> options)
        : base(options)
    {
    }

    public DbSet<LogEntry> Logs { get; set; } = null!;
}