namespace LogStreamX.Worker.Models;

public class KafkaLogDto
{
    public Guid EventId { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}