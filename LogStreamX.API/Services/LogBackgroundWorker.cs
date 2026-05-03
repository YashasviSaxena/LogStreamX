using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using LogStreamX.Infrastructure.Data;
using LogStreamX.Infrastructure.Models;

namespace LogStreamX.API.Services
{
    public class LogBackgroundWorker : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public LogBackgroundWorker(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _scopeFactory.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<LogDbContext>();

                // Example: process unprocessed logs
                var logs = db.LogEntries
                    .Where(x => !x.IsProcessed)
                    .ToList();

                foreach (var log in logs)
                {
                    log.IsProcessed = true;
                    log.ProcessedAt = DateTime.UtcNow;
                }

                if (logs.Any())
                {
                    await db.SaveChangesAsync();
                }

                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}