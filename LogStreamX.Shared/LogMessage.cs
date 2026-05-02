namespace LogStreamX.Shared
{
    public class LogMessage
    {
        public string EventId { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}