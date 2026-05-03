namespace LogStreamX.Contracts
{
    public class LogDto
    {
        public string EventId { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string Source { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}