using LogStreamX.Infrastructure.Data;
using LogStreamX.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LogStreamX.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LogsController : ControllerBase
    {
        private readonly AppDbContext _db;

        public LogsController(AppDbContext db)
        {
            _db = db;
        }

        // ================= GET LOGS =================
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var logs = await _db.LogEntries
                    .OrderByDescending(x => x.CreatedAt)
                    .Take(100)
                    .ToListAsync();

                return Ok(logs);
            }
            catch (Exception ex)
            {
                // 🔥 NEVER BREAK UI
                return Ok(new List<object>
                {
                    new
                    {
                        error = ex.Message,
                        source = "GetLogs"
                    }
                });
            }
        }

        // ================= CREATE LOG =================
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] LogEntry log)
        {
            try
            {
                if (log == null)
                    return BadRequest(new { error = "Invalid payload" });

                var entity = new LogEntry
                {
                    EventId = log.EventId ?? "unknown",
                    Message = log.Message ?? "",
                    Source = log.Source ?? "api",
                    CreatedAt = DateTime.UtcNow
                };

                _db.LogEntries.Add(entity);
                await _db.SaveChangesAsync();

                return Ok(new
                {
                    status = "saved",
                    entity.EventId
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    error = ex.Message,
                    source = "CreateLog"
                });
            }
        }

        // ================= DEBUG =================
        [HttpGet("debug")]
        public IActionResult Debug()
        {
            return Ok(new
            {
                status = "Logs API OK",
                time = DateTime.UtcNow
            });
        }
    }
}