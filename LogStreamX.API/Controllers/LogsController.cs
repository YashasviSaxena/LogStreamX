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

        // GET: /api/logs
        [HttpGet]
        public async Task<IActionResult> GetLogs()
        {
            try
            {
                var logs = await _context.LogEntries
                    .OrderByDescending(x => x.CreatedAt)
                    .ToListAsync();

                return Ok(logs);
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    error = true,
                    message = ex.Message
                });
            }
        }

        // POST: /api/logs
        [HttpPost]
        public async Task<IActionResult> AddLog([FromBody] LogEntry log)
        {
            try
            {
                if (log == null)
                    return BadRequest("Invalid log");

                log.CreatedAt = DateTime.UtcNow;

                await _context.LogEntries.AddAsync(log);
                await _context.SaveChangesAsync();

                return Ok(log);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    error = true,
                    message = ex.Message
                });
            }
        }

        // DEBUG
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