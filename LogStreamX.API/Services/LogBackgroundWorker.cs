using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using LogStreamX.Infrastructure.Data;

namespace LogStreamX.API.Services;

public class LogBackgroundWorker : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<LogBackgroundWorker> _logger;

    public LogBackgroundWorker(
        IServiceScopeFactory scopeFactory,
        ILogger<LogBackgroundWorker> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Worker started");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<LogDbContext>();

                var logs = db.Logs.ToList();

                _logger.LogInformation($"Logs count: {logs.Count}");

                await Task.Delay(5000, stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Worker error");
            }
        }
    }
}