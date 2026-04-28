namespace LogStreamX.API.Models;

public class LogRequest
{
    public string Message { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
