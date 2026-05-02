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

        // GET: api/logs
        [HttpGet]
        public async Task<IActionResult> GetLogs()
        {
            try
            {
                var logs = await _context.LogEntries
                    .OrderByDescending(x => x.CreatedAt)
                    .Take(100)
                    .ToListAsync();

                return Ok(logs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    error = "Database error",
                    message = ex.Message
                });
            }
        }

        // OPTIONAL TEST ENDPOINT
        [HttpGet("health")]
        public IActionResult Health()
        {
            return Ok(new
            {
                status = "Logs API OK",
                time = DateTime.UtcNow
            });
        }
    }
}