using Microsoft.AspNetCore.Mvc;
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

        // GET: /api/Logs
        [HttpGet]
        public IActionResult GetLogs()
        {
            try
            {
                var logs = _context.LogEntries
                    .OrderByDescending(x => x.CreatedAt)
                    .ToList();

                return Ok(logs);
            }
            catch (Exception ex)
            {
                return Ok(new[]
                {
                    new { error = ex.Message }
                });
            }
        }

        // POST: /api/Logs
        [HttpPost]
        public IActionResult AddLog(LogEntry log)
        {
            try
            {
                log.CreatedAt = DateTime.UtcNow;

                _context.LogEntries.Add(log);
                _context.SaveChanges();

                return Ok(log);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}