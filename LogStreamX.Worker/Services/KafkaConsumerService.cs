using Confluent.Kafka;
using LogStreamX.Infrastructure.Data;
using LogStreamX.Infrastructure.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace LogStreamX.Worker.Services
{
    public class KafkaConsumerService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<KafkaConsumerService> _logger;
        private readonly string _topic;

        public KafkaConsumerService(
            IServiceScopeFactory scopeFactory,
            ILogger<KafkaConsumerService> logger,
            IConfiguration config)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
            _topic = config["Kafka:Topic"]!;
        }

        // ✅ MUST BE PUBLIC (THIS FIXES YOUR ERROR)
        public void StartConsuming(CancellationToken cancellationToken)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = "your-bootstrap",
                GroupId = "logstreamx-worker-group",
                AutoOffsetReset = AutoOffsetReset.Earliest,
                SecurityProtocol = SecurityProtocol.SaslSsl,
                SaslMechanism = SaslMechanism.Plain,
                SaslUsername = "YOUR_API_KEY",
                SaslPassword = "YOUR_API_SECRET"
            };

            using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
            consumer.Subscribe(_topic);

            _logger.LogInformation("🔥 Kafka Consumer started...");

            while (!cancellationToken.IsCancellationRequested)
            {
                var cr = consumer.Consume(cancellationToken);

                var log = JsonSerializer.Deserialize<LogEntry>(cr.Message.Value);

                if (log != null)
                {
                    using var scope = _scopeFactory.CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<LogDbContext>();

                    db.LogEntries.Add(log);
                    db.SaveChanges();

                    _logger.LogInformation("✅ Saved to DB");
                }
            }
        }
    }
}