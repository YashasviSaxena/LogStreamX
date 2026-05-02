using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace LogStreamX.API.Services
{
    public class KafkaProducerService
    {
        private readonly IProducer<Null, string> _producer;
        private readonly string _topic;

        public KafkaProducerService(IConfiguration config)
        {
            var bootstrap = config["Kafka:BootstrapServers"] ?? "";
            var apiKey = config["Kafka:ApiKey"] ?? "";
            var apiSecret = config["Kafka:ApiSecret"] ?? "";

            _topic = config["Kafka:Topic"] ?? "logs-topic";

            var conf = new ProducerConfig
            {
                BootstrapServers = bootstrap,
                SecurityProtocol = SecurityProtocol.SaslSsl,
                SaslMechanism = SaslMechanism.Plain,
                SaslUsername = apiKey,
                SaslPassword = apiSecret
            };

            _producer = new ProducerBuilder<Null, string>(conf).Build();
        }

        // ✅ FINAL METHOD NAME (USE THIS EVERYWHERE)
        public async Task SendMessageAsync(object log)
        {
            var json = JsonSerializer.Serialize(log);

            await _producer.ProduceAsync(_topic, new Message<Null, string>
            {
                Value = json
            });
        }
    }
}