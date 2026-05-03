using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LogStreamX.Infrastructure.Data;
using LogStreamX.Infrastructure.Models;

namespace LogStreamX.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LogsController : ControllerBase
{
    private readonly LogDbContext _context;

    public LogsController(LogDbContext context)
    {
        _context = context;
    }

    // GET: api/logs
    [HttpGet]
    public async Task<ActionResult<List<LogEntry>>> GetAllLogs()
    {
        var logs = await _context.Logs
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();

        return Ok(logs);
    }

    // GET: api/logs/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<LogEntry>> GetLogById(int id)
    {
        var log = await _context.Logs.FirstOrDefaultAsync(x => x.Id == id);

        if (log == null)
            return NotFound(new { message = "Log not found" });

        return Ok(log);
    }

    // GET: api/logs/source/{source}
    [HttpGet("source/{source}")]
    public async Task<ActionResult<List<LogEntry>>> GetLogsBySource(string source)
    {
        var logs = await _context.Logs
            .Where(x => x.Source == source)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();

        return Ok(logs);
    }

    // DELETE: api/logs/clear
    [HttpDelete("clear")]
    public async Task<IActionResult> ClearLogs()
    {
        var logs = await _context.Logs.ToListAsync();

        _context.Logs.RemoveRange(logs);
        await _context.SaveChangesAsync();

        return Ok(new { message = "All logs cleared" });
    }
}