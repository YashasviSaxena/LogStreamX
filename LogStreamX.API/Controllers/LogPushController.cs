using Microsoft.AspNetCore.Mvc;
using LogStreamX.API.Services;
using LogStreamX.Contracts;

namespace LogStreamX.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LogPushController : ControllerBase
    {
        private readonly KafkaProducerService _kafka;

        public LogPushController(KafkaProducerService kafka)
        {
            _kafka = kafka;
        }

        [HttpPost]
        public async Task<IActionResult> PushLog([FromBody] LogEvent request)
        {
            if (request == null)
                return BadRequest("Invalid request");

            await _kafka.SendMessageAsync(request);

            return Ok("✅ Sent to Kafka");
        }
    }
}