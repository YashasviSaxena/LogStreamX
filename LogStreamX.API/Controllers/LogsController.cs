using Microsoft.AspNetCore.Mvc;
using LogStreamX.Infrastructure.Data;
using LogStreamX.Infrastructure.Models;
using LogStreamX.Contracts;

namespace LogStreamX.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LogsController : ControllerBase
    {
        private readonly LogDbContext _db;

        public LogsController(LogDbContext db)
        {
            _db = db;
        }

        // GET: /api/logs
        [HttpGet]
        public IActionResult GetLogs()
        {
            var logs = _db.LogEntries
                .OrderByDescending(x => x.CreatedAt)
                .ToList();

            return Ok(logs);
        }

        // POST: /api/logs (manual test fallback)
        [HttpPost]
        public IActionResult PostLog(LogDto dto)
        {
            var entity = new LogEntry
            {
                EventId = dto.EventId,
                Message = dto.Message,
                Source = dto.Source,
                CreatedAt = dto.CreatedAt
            };

            _db.LogEntries.Add(entity);
            _db.SaveChanges();

            return Ok(new { status = "saved", entity });
        }
    }
}