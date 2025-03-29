namespace Application.CQRS.ActivityLogs.DTOs;

public class ActivityLogListDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int EntityId { get; set; }
    public string Action { get; set; } = null!;
    public string EntityType { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
}
