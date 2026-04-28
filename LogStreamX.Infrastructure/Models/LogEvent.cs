namespace LogStreamX.Contracts;

public class LogEvent
{
    public string EventId { get; set; }
    public string Message { get; set; }
    public DateTime CreatedAt { get; set; }
}