using LogStreamX.Infrastructure.Data;
using LogStreamX.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;

namespace LogStreamX.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LogPushController : ControllerBase
    {
        private readonly LogDbContext _db;

        public LogPushController(LogDbContext db)
        {
            _db = db;
        }

        [HttpPost]
        public async Task<IActionResult> PushLog([FromBody] LogEntry log)
        {
            log.CreatedAt = DateTime.UtcNow;

            _db.LogEntries.Add(log);
            await _db.SaveChangesAsync();

            return Ok(new { message = "Log stored successfully" });
        }
    }
}