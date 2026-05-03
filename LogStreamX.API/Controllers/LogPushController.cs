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
        public async Task<IActionResult> Push([FromBody] LogDto dto)
        {
            var producerConfig = new ProducerConfig
            {
                BootstrapServers = _config["Kafka:BootstrapServers"],

                // 🔥 REQUIRED FOR CONFLUENT CLOUD
                SecurityProtocol = SecurityProtocol.SaslSsl,
                SaslMechanism = SaslMechanism.Plain,
                SaslUsername = _config["Kafka:ApiKey"],
                SaslPassword = _config["Kafka:ApiSecret"]
            };

            using var producer = new ProducerBuilder<Null, string>(producerConfig).Build();

            var json = JsonSerializer.Serialize(dto);

            await producer.ProduceAsync(_config["Kafka:Topic"], new Message<Null, string>
            {
                Value = json
            });

            return Ok(new { status = "received", dto });
        }
    }
}