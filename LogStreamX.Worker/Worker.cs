using Microsoft.Extensions.Hosting;
using LogStreamX.Worker.Services;

public class Worker : BackgroundService
{
    private readonly KafkaConsumerService _kafka;

    public Worker(KafkaConsumerService kafka)
    {
        _kafka = kafka;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // ❌ NO StartAsync / private calls
        _kafka.StartConsuming(stoppingToken);
        return Task.CompletedTask;
    }
}