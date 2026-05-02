using Microsoft.EntityFrameworkCore;
using LogStreamX.Infrastructure.Models;

namespace LogStreamX.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<LogEntry> LogEntries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LogEntry>(entity =>
            {
                entity.ToTable("LogEntries");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.EventId).HasMaxLength(100);
                entity.Property(x => x.Message).HasMaxLength(4000);
                entity.Property(x => x.Source).HasMaxLength(100);
                entity.Property(x => x.CreatedAt);
            });
        }
    }
}