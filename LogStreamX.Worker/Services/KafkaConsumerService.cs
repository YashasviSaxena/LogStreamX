using Confluent.Kafka;
using LogStreamX.Contracts;
using LogStreamX.Infrastructure.Data;
using LogStreamX.Infrastructure.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace LogStreamX.Worker.Services
{
    public class KafkaConsumerService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<KafkaConsumerService> _logger;
        private readonly IConfiguration _config;

        public KafkaConsumerService(
            IServiceScopeFactory scopeFactory,
            ILogger<KafkaConsumerService> logger,
            IConfiguration config)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
            _config = config;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.Run(() => Consume(stoppingToken), stoppingToken);
        }

        private void Consume(CancellationToken cancellationToken)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = _config["Kafka:BootstrapServers"],
                GroupId = _config["Kafka:GroupId"],
                AutoOffsetReset = AutoOffsetReset.Earliest,

                // 🔥 REQUIRED FOR CONFLUENT CLOUD
                SecurityProtocol = SecurityProtocol.SaslSsl,
                SaslMechanism = SaslMechanism.Plain,
                SaslUsername = _config["Kafka:ApiKey"],
                SaslPassword = _config["Kafka:ApiSecret"]
            };

            using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
            consumer.Subscribe(_config["Kafka:Topic"]);

            _logger.LogInformation("🚀 Kafka Consumer Started");

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var cr = consumer.Consume(cancellationToken);

                    var logDto = JsonSerializer.Deserialize<LogDto>(cr.Message.Value);

                    if (logDto == null) continue;

                    using var scope = _scopeFactory.CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<LogDbContext>();

                    var entity = new LogEntry
                    {
                        EventId = logDto.EventId ?? "",
                        Message = logDto.Message ?? "",
                        CreatedAt = logDto.CreatedAt,
                        Source = logDto.Source ?? ""
                    };

                    db.LogEntries.Add(entity);
                    db.SaveChanges();

                    _logger.LogInformation("✅ Saved log: {EventId}", entity.EventId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "❌ Kafka Error");
                }
            }
        }
    }
}