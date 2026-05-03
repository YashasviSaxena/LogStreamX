using Confluent.Kafka;
using Microsoft.Extensions.Configuration;

namespace LogStreamX.Worker.Services
{
    public class KafkaProducerService
    {
        private readonly IProducer<Null, string> _producer;
        private readonly string _topic;

        public KafkaProducerService(IConfiguration config)
        {
            var kafkaConfig = new ProducerConfig
            {
                BootstrapServers = config["Kafka:BootstrapServers"],
                SecurityProtocol = SecurityProtocol.SaslSsl,
                SaslMechanism = SaslMechanism.Plain,
                SaslUsername = config["Kafka:ApiKey"],
                SaslPassword = config["Kafka:ApiSecret"]
            };

            _producer = new ProducerBuilder<Null, string>(kafkaConfig).Build();
            _topic = config["Kafka:Topic"]!;
        }

        public async Task SendMessageAsync(string message)
        {
            await _producer.ProduceAsync(_topic,
                new Message<Null, string> { Value = message });
        }
    }
}
