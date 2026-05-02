using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LogStreamX.Infrastructure.Data;
using LogStreamX.Infrastructure.Models;

namespace LogStreamX.API.Controllers;

[ApiController]
[Route("api/logs")]
public class LogsController : ControllerBase
{
    private readonly AppDbContext _context;

    public LogsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        try
        {
            if (_context == null)
                return Ok(new List<object>());

            var logs = await _context.LogEntries
                .OrderByDescending(x => x.CreatedAt)
                .Take(200)
                .ToListAsync();

            return Ok(logs);
        }
        catch (Exception ex)
        {
            // NEVER crash frontend with 500 HTML page
            return Ok(new
            {
                error = true,
                message = ex.Message
            });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Add(LogEntry log)
    {
        try
        {
            log.CreatedAt = DateTime.UtcNow;

            _context.LogEntries.Add(log);
            await _context.SaveChangesAsync();

            return Ok(new { success = true });
        }
        catch (Exception ex)
        {
            return Ok(new { error = true, message = ex.Message });
        }
    }
}