using Microsoft.EntityFrameworkCore;


namespace LogStreamX.Infrastructure
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
            modelBuilder.Entity<LogEntry>()
                .HasIndex(x => x.EventId)
                .IsUnique();
        }
    }
}