using Confluent.Kafka;
using System.Text.Json;

namespace LogStreamX.API.Services
{
    public class KafkaProducerService
    {
        private readonly IConfiguration _config;
        private readonly IProducer<Null, string> _producer;

        public KafkaProducerService(IConfiguration config)
        {
            _config = config;

            var kafkaConfig = new ProducerConfig
            {
                BootstrapServers = _config["Kafka:BootstrapServers"]
            };

            _producer = new ProducerBuilder<Null, string>(kafkaConfig).Build();
        }

        public async Task SendLogAsync(object logRequest)
        {
            var message = new Message<Null, string>
            {
                Value = JsonSerializer.Serialize(logRequest)
            };

            await _producer.ProduceAsync("logs-topic", message);
        }
    }
}