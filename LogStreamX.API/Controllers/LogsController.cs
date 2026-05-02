using Microsoft.AspNetCore.Mvc;
using LogStreamX.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LogStreamX.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LogsController : ControllerBase
    {
        private readonly LogDbContext _context;

        public LogsController(LogDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetLogs()
        {
            try
            {
                var logs = await _context.LogEntries
                    .OrderByDescending(x => x.Id)
                    .Take(100)
                    .ToListAsync();

                return Ok(logs);
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