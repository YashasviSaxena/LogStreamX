using Microsoft.AspNetCore.Mvc;
using LogStreamX.Infrastructure;
using LogStreamX.API.Models;
using LogStreamX.API.Services;

namespace LogStreamX.API.Controllers
{
    [ApiController]
    [Route("api/logpush")]
    public class LogPushController : ControllerBase
    {
        private readonly KafkaProducerService _kafka;
        private readonly AppDbContext _db;

        public LogPushController(KafkaProducerService kafka, AppDbContext db)
        {
            _kafka = kafka;
            _db = db;
        }

        [HttpPost("push")]
        public async Task<IActionResult> Push([FromBody] LogDto dto)
        {
            // send to kafka
            await _kafka.SendMessage(System.Text.Json.JsonSerializer.Serialize(dto));

            // ✅ SAVE CLEAN DATA ONLY (IMPORTANT FIX)
            var log = new LogEntry
            {
                EventId = dto.EventId,
                Message = dto.Message,
                CreatedAt = dto.CreatedAt,
                Source = "kafka"
            };

            _db.LogEntries.Add(log);
            await _db.SaveChangesAsync();

            return Ok(new { status = "sent + saved" });
        }
    }
}