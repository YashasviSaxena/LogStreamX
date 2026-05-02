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

        // GET: api/logs
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                if (_context == null || _context.LogEntries == null)
                {
                    return Ok(new List<object>());
                }

                var logs = await _context.LogEntries
                    .OrderByDescending(x => x.CreatedAt)
                    .Take(100)
                    .ToListAsync();

                return Ok(logs);
            }
            catch (Exception ex)
            {
                // NEVER crash UI again
                return Ok(new
                {
                    error = true,
                    message = ex.Message
                });
            }
        }

        // DEBUG ENDPOINT
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