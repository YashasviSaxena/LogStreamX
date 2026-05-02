using LogStreamX.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LogStreamX.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LogsController : ControllerBase
    {
        private readonly LogDbContext _db;

        public LogsController(LogDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetLogs()
        {
            var logs = await _db.LogEntries
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();

            return Ok(logs);
        }
    }
}