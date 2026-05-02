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

        [HttpGet]
        public async Task<IActionResult> GetLogs()
        {
            var logs = await _context.LogEntries
                .OrderByDescending(x => x.CreatedAt)
                .Take(100)
                .ToListAsync();

            return Ok(logs);
        }

        [HttpPost]
        public async Task<IActionResult> AddLog(LogEntry log)
        {
            log.CreatedAt = DateTime.UtcNow;

            _context.LogEntries.Add(log);
            await _context.SaveChangesAsync();

            return Ok(log);
        }
    }
}