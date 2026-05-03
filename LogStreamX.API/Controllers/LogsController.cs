using Microsoft.AspNetCore.Mvc;

namespace LogStreamX.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LogsController : ControllerBase
    {
        private static readonly List<object> _logs = new();

        [HttpGet]
        public IActionResult GetLogs()
        {
            return Ok(_logs);
        }

        [HttpPost]
        public IActionResult AddLog([FromBody] object log)
        {
            _logs.Add(log);
            return Ok(new { status = "stored in memory" });
        }
    }
}