using Microsoft.AspNetCore.Mvc;
using LogStreamX.Infrastructure.Data;

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
            try
            {
                var logs = _db.LogEntries
                    .OrderByDescending(x => x.CreatedAt)
                    .Take(100)
                    .ToList();

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