using Confluent.Kafka;
using LogStreamX.Contracts;
using LogStreamX.Infrastructure.Data;
using LogStreamX.Infrastructure.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace LogStreamX.Worker.Services;

public class KafkaConsumerService : BackgroundService
{
    private readonly ILogger<KafkaConsumerService> _logger;
    private readonly IConfiguration _configuration;
    private readonly IServiceScopeFactory _scopeFactory;

    public KafkaConsumerService(
        ILogger<KafkaConsumerService> logger,
        IConfiguration configuration,
        IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _configuration = configuration;
        _scopeFactory = scopeFactory;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return Task.Run(() =>
        {
            var kafka = _configuration.GetSection("Kafka");

            var config = new ConsumerConfig
            {
                BootstrapServers = kafka["BootstrapServers"],
                GroupId = kafka["GroupId"],
                AutoOffsetReset = AutoOffsetReset.Earliest,

                SecurityProtocol = SecurityProtocol.SaslSsl,
                SaslMechanism = SaslMechanism.Plain,
                SaslUsername = kafka["ApiKey"],
                SaslPassword = kafka["ApiSecret"]
            };

            using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
            consumer.Subscribe(kafka["Topic"]);

            _logger.LogInformation("🚀 Kafka Consumer Started");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var result = consumer.Consume(stoppingToken);

                    if (result?.Message?.Value == null)
                        continue;

                    var logEvent = JsonSerializer.Deserialize<LogEvent>(result.Message.Value);

                    if (logEvent == null)
                    {
                        _logger.LogWarning("⚠️ Invalid Kafka message received");
                        continue;
                    }

                    using var scope = _scopeFactory.CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<LogDbContext>();

                    var entity = new LogEntry
                    {
                        EventId = logEvent.EventId,
                        Message = logEvent.Message,
                        Source = logEvent.Source,
                        CreatedAt = logEvent.CreatedAt
                    };

                    // ✅ FIXED: Correct DbSet name
                    db.Logs.Add(entity);
                    db.SaveChanges();

                    _logger.LogInformation($"✅ Saved Log: {logEvent.EventId}");
                }
                catch (ConsumeException ex)
                {
                    _logger.LogError(ex, "❌ Kafka consume error");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "❌ Worker error");
                }
            }

            consumer.Close();
        }, stoppingToken);
    }
}