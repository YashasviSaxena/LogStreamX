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

        // ✅ SAFE GET (UI NEVER BREAKS)
        [HttpGet]
        public async Task<IActionResult> GetLogs()
        {
            try
            {
                var logs = await _context.LogEntries
                    .OrderByDescending(x => x.CreatedAt)
                    .Take(200)
                    .ToListAsync();

                return Ok(logs);
            }
            catch (Exception ex)
            {
                // ❌ NEVER BREAK UI
                return Ok(new List<LogEntry>());
            }
        }

        // ✅ SAVE LOG
        [HttpPost]
        public async Task<IActionResult> CreateLog(LogEntry log)
        {
            log.CreatedAt = DateTime.UtcNow;

            _context.LogEntries.Add(log);
            await _context.SaveChangesAsync();

            return Ok(new { success = true });
        }
    }
}