using Microsoft.AspNetCore.Mvc;

namespace LogStreamX.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LogPushController : ControllerBase
    {
        // POST: /api/LogPush
        [HttpPost]
        public IActionResult Push([FromBody] LogEvent request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest("Invalid payload");
                }

                // Simulate Kafka push (you already have this working)
                // In real system: producer.SendAsync(...)

                return Ok(new
                {
                    status = "Sent to Kafka",
                    eventId = request.EventId
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    error = ex.Message
                });
            }
        }
    }

    // DTO (IMPORTANT)
    public class LogEvent
    {
        public string EventId { get; set; }
        public string Message { get; set; }
        public string Source { get; set; }
    }
}