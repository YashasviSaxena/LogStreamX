using Confluent.Kafka;
using LogStreamX.Contracts;
using LogStreamX.Infrastructure.Data;
using LogStreamX.Infrastructure.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace LogStreamX.Worker.Services;

public class KafkaConsumerService : BackgroundService
{
    private readonly ILogger<KafkaConsumerService> _logger;
    private readonly IConfiguration _config;
    private readonly IServiceScopeFactory _scopeFactory;

    public KafkaConsumerService(
        ILogger<KafkaConsumerService> logger,
        IConfiguration config,
        IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _config = config;
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var kafka = _config.GetSection("Kafka");

        var conf = new ConsumerConfig
        {
            BootstrapServers = kafka["BootstrapServers"],
            GroupId = kafka["GroupId"],
            AutoOffsetReset = AutoOffsetReset.Earliest,

            SecurityProtocol = SecurityProtocol.SaslSsl,
            SaslMechanism = SaslMechanism.Plain,
            SaslUsername = kafka["ApiKey"],
            SaslPassword = kafka["ApiSecret"]
        };

        using var consumer = new ConsumerBuilder<Ignore, string>(conf).Build();
        consumer.Subscribe(kafka["Topic"]);

        _logger.LogInformation("🚀 Kafka Consumer Started");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var result = consumer.Consume(stoppingToken);

                var logEvent = JsonSerializer.Deserialize<LogEvent>(result.Message.Value);

                if (logEvent != null)
                {
                    using var scope = _scopeFactory.CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<LogDbContext>();

                    var entity = new LogEntry
                    {
                        EventId = logEvent.EventId,
                        Message = logEvent.Message,
                        Source = logEvent.Source,
                        CreatedAt = logEvent.CreatedAt
                    };

                    // ✅ FINAL FIX (USE Logs)
                    db.Logs.Add(entity);

                    await db.SaveChangesAsync();

                    _logger.LogInformation($"✅ Saved log: {logEvent.EventId}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Kafka Error");
            }
        }
    }
}