namespace LogStreamX.Infrastructure.Models;

public class FailedLog
{
    public int Id { get; set; }

    public string Payload { get; set; } = string.Empty;

    public int RetryCount { get; set; }

    public DateTime FailedAt { get; set; } = DateTime.UtcNow;
}