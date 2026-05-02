using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LogStreamX.Infrastructure.Data;

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
            var logs = await _context.LogEntries
                .OrderByDescending(x => x.CreatedAt)
                .Take(100)
                .ToListAsync();

            return Ok(logs);
        }

        // OPTIONAL: GET single log by id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetLog(int id)
        {
            var log = await _context.LogEntries
                .FirstOrDefaultAsync(x => x.Id == id);

            if (log == null)
                return NotFound();

            return Ok(log);
        }

        // OPTIONAL: health check endpoint
        [HttpGet("health")]
        public IActionResult Health()
        {
            return Ok(new { status = "LogStreamX API Running 🚀" });
        }
    }
}