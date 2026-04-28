using Confluent.Kafka;
using Microsoft.Extensions.Configuration;

namespace LogStreamX.API.Services
{
    public class KafkaProducerService
    {
        private readonly IProducer<Null, string> _producer;
        private readonly string _topic = "logs-topic";

        public KafkaProducerService(IConfiguration config)
        {
            var bootstrapServers = config["Kafka:BootstrapServers"] ?? "localhost:9092";

            var producerConfig = new ProducerConfig
            {
                BootstrapServers = bootstrapServers,

                // 🔥 SAFE DEFAULTS (works local + cloud)
                Acks = Acks.All,
                EnableIdempotence = true
            };

            _producer = new ProducerBuilder<Null, string>(producerConfig).Build();
        }

        public async Task SendMessage(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                return;

            try
            {
                var result = await _producer.ProduceAsync(_topic,
                    new Message<Null, string>
                    {
                        Value = message
                    });

                Console.WriteLine($"Kafka sent: {result.TopicPartitionOffset}");
            }
            catch (ProduceException<Null, string> ex)
            {
                Console.WriteLine($"Kafka error: {ex.Error.Reason}");
            }
        }
    }
}