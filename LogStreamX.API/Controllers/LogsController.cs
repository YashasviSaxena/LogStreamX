using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LogStreamX.Infrastructure.Data;
using LogStreamX.Infrastructure.Models;

namespace LogStreamX.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LogsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public LogsController(AppDbContext context)
        {
            _context = context;
        }

        // GET ALL LOGS
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var logs = await _context.LogEntries
                    .OrderByDescending(x => x.Id)
                    .ToListAsync();

                return Ok(logs);
            }
            catch (Exception ex)
            {
                return Ok(new List<object>
                {
                    new { error = ex.Message }
                });
            }
        }

        // POST LOG (FIXED)
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] LogEntry log)
        {
            try
            {
                if (log == null)
                    return BadRequest("Invalid log");

                var entity = new LogEntry
                {
                    EventId = log.EventId ?? "NA",
                    Message = log.Message ?? "NA",
                    Source = log.Source ?? "swagger",
                    CreatedAt = DateTime.UtcNow
                };

                _context.LogEntries.Add(entity);
                await _context.SaveChangesAsync();

                return Ok(new { status = "saved" });
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
}