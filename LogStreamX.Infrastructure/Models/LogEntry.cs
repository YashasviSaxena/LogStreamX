namespace LogStreamX.Infrastructure.Models;

public class LogEntry
{
    public int Id { get; set; }

    public string EventId { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string Source { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
}