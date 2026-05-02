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

        // =========================
        // GET: api/logs
        // =========================
        [HttpGet]
        public async Task<IActionResult> GetLogs()
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
                // 🔥 ALWAYS RETURN SAFE JSON (prevents UI crash)
                return StatusCode(500, new
                {
                    error = ex.Message,
                    source = "GetLogs"
                });
            }
        }

        // =========================
        // POST: api/logs
        // =========================
        [HttpPost]
        public async Task<IActionResult> CreateLog([FromBody] LogEntry request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest(new { error = "Request body is null" });
                }

                var log = new LogEntry
                {
                    EventId = request.EventId ?? "unknown",
                    Message = request.Message ?? "empty",
                    Source = request.Source ?? "api",
                    CreatedAt = DateTime.UtcNow
                };

                _db.LogEntries.Add(log);
                await _db.SaveChangesAsync();

                return Ok(new
                {
                    status = "saved",
                    log.EventId
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

        // =========================
        // DEBUG: api/logs/debug
        // =========================
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