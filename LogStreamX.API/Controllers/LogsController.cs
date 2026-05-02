using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LogStreamX.Infrastructure.Data;
using System.Text.Json;

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
            try
            {
                var logs = await _db.LogEntries
                    .OrderByDescending(x => x.CreatedAt)
                    .Take(100)
                    .ToListAsync();

                var result = logs.Select(x =>
                {
                    string message = x.Message;
                    string eventId = x.EventId;

                    // ✅ HANDLE OLD JSON STORED AS STRING
                    if (!string.IsNullOrEmpty(x.Message) && x.Message.StartsWith("{"))
                    {
                        try
                        {
                            var parsed = JsonSerializer.Deserialize<Dictionary<string, object>>(x.Message);

                            if (parsed != null)
                            {
                                message = parsed.ContainsKey("message") ? parsed["message"]?.ToString() ?? "" : message;
                                eventId = parsed.ContainsKey("eventId") ? parsed["eventId"]?.ToString() ?? "" : eventId;
                            }
                        }
                        catch
                        {
                            // ignore bad json
                        }
                    }

                    return new
                    {
                        id = x.Id,
                        message = message ?? "",
                        eventId = eventId ?? "",
                        createdAt = x.CreatedAt
                    };
                });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"API ERROR: {ex.Message}");
            }
        }
    }
}