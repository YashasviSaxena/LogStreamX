namespace LogStreamX.API.Models
{
    public class LogEvent
    {
        public string EventId { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string Source { get; set; } = "swagger";

        // REQUIRED FIELD
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}