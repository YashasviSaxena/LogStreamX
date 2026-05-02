using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace LogStreamX.Infrastructure.Data
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

            var connectionString =
                "Host=dpg-d7r3b60sfn5c73c8l09g-a;Port=5432;Database=logstreamxdb;Username=logstreamxdb_user;Password=4R8AtI0gmPWEk6yejgO6CPRCYjIvbcM5";

            optionsBuilder.UseNpgsql(connectionString);

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}