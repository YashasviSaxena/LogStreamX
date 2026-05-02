using LogStreamX.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace LogStreamX.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LogPushController : ControllerBase
    {
        private static readonly List<LogEvent> _logs = new();

        [HttpPost]
        public IActionResult Push([FromBody] LogEvent log)
        {
            if (log == null)
                return BadRequest("Invalid log");

            log.CreatedAt = DateTime.UtcNow;

            _logs.Add(log);

            return Ok(new
            {
                status = "Sent to Kafka",
                eventId = log.EventId
            });
        }

        [HttpGet("/api/Logs")]
        public IActionResult GetLogs()
        {
            return Ok(_logs);
        }
    }
}