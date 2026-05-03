namespace LogStreamX.Infrastructure.Models
{
    public class LogEntry
    {
        public int Id { get; set; }

        public string EventId { get; set; } = Guid.NewGuid().ToString();

        public string Message { get; set; } = string.Empty;

        public string Source { get; set; } = "API";

        public string Level { get; set; } = "INFO";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool IsProcessed { get; set; } = false;

        public DateTime? ProcessedAt { get; set; }
    }
}