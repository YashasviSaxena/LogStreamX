using Microsoft.AspNetCore.Mvc;
using LogStreamX.Infrastructure.Data;
using LogStreamX.Infrastructure.Models;
using LogStreamX.Contracts;

namespace LogStreamX.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LogPushController : ControllerBase
    {
        private readonly AppDbContext _db;

        public LogPushController(AppDbContext db)
        {
            _db = db;
        }

        [HttpPost]
        public async Task<IActionResult> Push(LogEvent log)
        {
            var entity = new LogEntry
            {
                EventId = log.EventId,
                Message = log.Message,
                Source = log.Source,
                CreatedAt = DateTime.UtcNow
            };

            _db.LogEntries.Add(entity);
            await _db.SaveChangesAsync();

            return Ok(new
            {
                status = "Sent to DB",
                eventId = log.EventId
            });
        }
    }
}