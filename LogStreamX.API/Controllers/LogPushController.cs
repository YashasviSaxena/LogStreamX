using Microsoft.AspNetCore.Mvc;
using LogStreamX.Infrastructure.Data;
using LogStreamX.Infrastructure.Models;

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
        public async Task<IActionResult> Push(LogEntry log)
        {
            log.CreatedAt = DateTime.UtcNow;

            _db.Logs.Add(log);
            await _db.SaveChangesAsync();

            return Ok("Saved");
        }
    }
}