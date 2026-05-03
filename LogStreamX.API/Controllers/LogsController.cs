using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LogStreamX.Infrastructure.Data;

namespace LogStreamX.API.Controllers;

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
    public async Task<IActionResult> Get()
    {
        try
        {
            var logs = await _db.LogEntries
                .OrderByDescending(x => x.CreatedAt)
                .Take(200)
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
}