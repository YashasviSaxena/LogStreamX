using Confluent.Kafka;
using System.Text.Json;
using LogStreamX.Contracts;
using LogStreamX.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace LogStreamX.Worker;

public class Worker : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public Worker(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var config = new ConsumerConfig
        {
            BootstrapServers = "localhost:9092",
            GroupId = "logstream-group",
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnableAutoCommit = false
        };

        using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
        consumer.Subscribe("logs-topic");

        Console.WriteLine("🚀 Worker started");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var result = consumer.Consume(stoppingToken);

                Console.WriteLine($"📩 Received: {result.Message.Value}");

                using var scope = _scopeFactory.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                var log = JsonSerializer.Deserialize<LogEvent>(result.Message.Value);

                if (log == null)
                    continue;

                var exists = await db.LogEntries.AnyAsync(x => x.EventId == log.EventId);

                if (exists)
                {
                    Console.WriteLine("⚠ Duplicate skipped");
                    continue;
                }

                var entity = new LogEntry
                {
                    EventId = log.EventId,
                    Message = log.Message,
                    CreatedAt = log.CreatedAt,
                    Source = "kafka"
                };

                db.LogEntries.Add(entity);
                await db.SaveChangesAsync();

                Console.WriteLine("✅ Saved to DB");

                consumer.Commit(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ ERROR:");
                Console.WriteLine(ex.ToString());
            }
        }
    }
}