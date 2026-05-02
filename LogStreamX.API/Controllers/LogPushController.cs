using Confluent.Kafka;
using LogStreamX.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace LogStreamX.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LogPushController : ControllerBase
    {
        private readonly IConfiguration _config;

        public LogPushController(IConfiguration config)
        {
            _config = config;
        }

        [HttpPost]
        public async Task<IActionResult> Push(LogEvent log)
        {
            var producerConfig = new ProducerConfig
            {
                BootstrapServers = _config["Kafka:BootstrapServers"],
                SecurityProtocol = SecurityProtocol.SaslSsl,
                SaslMechanism = SaslMechanism.Plain,
                SaslUsername = _config["Kafka:ApiKey"],
                SaslPassword = _config["Kafka:ApiSecret"]
            };

            using var producer = new ProducerBuilder<Null, string>(producerConfig).Build();

            var message = JsonSerializer.Serialize(log);

            await producer.ProduceAsync(_config["Kafka:Topic"],
                new Message<Null, string> { Value = message });

            return Ok("Sent to Kafka");
        }
    }
}