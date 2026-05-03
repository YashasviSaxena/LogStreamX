using LogStreamX.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult GetLogs()
        {
            var logs = _db.LogEntries
                .OrderByDescending(x => x.CreatedAt)
                .ToList();

            return Ok(logs);
        }
    }
}