namespace LogStreamX.Contracts
{
    public class LogEvent
    {
        public string EventId { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;

        // ✅ ADD THIS (THIS FIXES YOUR ERROR)
        public string Source { get; set; } = "swagger";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}