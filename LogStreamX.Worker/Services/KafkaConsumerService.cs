using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using LogStreamX.Contracts;
using LogStreamX.Infrastructure.Data;
using LogStreamX.Infrastructure.Models;

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

        private void Consume(CancellationToken token)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = _config["Kafka:BootstrapServers"],
                GroupId = _config["Kafka:GroupId"],
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = true
            };

            using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
            consumer.Subscribe(_config["Kafka:Topic"]);

            _logger.LogInformation("🔥 Kafka Consumer started");

            while (!token.IsCancellationRequested)
            {
                var cr = consumer.Consume(token);

                var dto = JsonSerializer.Deserialize<LogDto>(cr.Message.Value);

                if (dto == null) continue;

                using var scope = _scopeFactory.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<LogDbContext>();

                db.LogEntries.Add(new LogEntry
                {
                    EventId = dto.EventId,
                    Message = dto.Message,
                    Source = dto.Source,
                    CreatedAt = dto.CreatedAt
                });

                db.SaveChanges();

                _logger.LogInformation("✅ Saved: {id}", dto.EventId);
            }
        }
    }
}