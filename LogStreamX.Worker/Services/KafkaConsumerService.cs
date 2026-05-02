using Confluent.Kafka;
using LogStreamX.Contracts;
using LogStreamX.Infrastructure.Data;
using LogStreamX.Infrastructure.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace LogStreamX.Worker.Services
{
    public class KafkaConsumerService
    {
        private readonly IConfiguration _configuration;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<KafkaConsumerService> _logger;

        public KafkaConsumerService(
            IConfiguration configuration,
            IServiceScopeFactory scopeFactory,
            ILogger<KafkaConsumerService> logger)
        {
            _configuration = configuration;
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        public async Task StartConsuming(CancellationToken cancellationToken)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = _configuration["Kafka:BootstrapServers"],
                GroupId = _configuration["Kafka:GroupId"],
                AutoOffsetReset = AutoOffsetReset.Earliest,

                SecurityProtocol = SecurityProtocol.SaslSsl,
                SaslMechanism = SaslMechanism.Plain,
                SaslUsername = _configuration["Kafka:ApiKey"],
                SaslPassword = _configuration["Kafka:ApiSecret"]
            };

            using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
            consumer.Subscribe(_configuration["Kafka:Topic"]); // logs-topic

            _logger.LogInformation("🔥 Kafka Consumer started...");

            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    var result = consumer.Consume(cancellationToken);
                    var message = result?.Message?.Value;

                    if (string.IsNullOrWhiteSpace(message))
                        continue;

                    _logger.LogInformation($"📩 Received: {message}");

                    try
                    {
                        var log = JsonSerializer.Deserialize<LogEvent>(message);

                        if (log == null)
                            continue;

                        using var scope = _scopeFactory.CreateScope();
                        var db = scope.ServiceProvider.GetRequiredService<LogDbContext>();

                        db.LogEntries.Add(new LogEntry
                        {
                            EventId = log.EventId,
                            Message = log.Message,
                            CreatedAt = log.CreatedAt,
                            Source = "kafka"
                        });

                        await db.SaveChangesAsync(cancellationToken);

                        _logger.LogInformation("✅ Saved to DB");
                    }
                    catch (JsonException)
                    {
                        _logger.LogWarning($"⚠️ Skipped non-JSON: {message}");
                    }
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("🛑 Kafka stopped.");
            }
        }
    }
}