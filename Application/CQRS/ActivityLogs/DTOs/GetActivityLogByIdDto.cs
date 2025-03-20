namespace Application.CQRS.ActivityLogs.DTOs;

public class GetActivityLogByIdDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Action { get; set; } = null!;
    public string EntityType { get; set; } = null!;
    public int EntityId { get; set; }
    public DateTime CreatedAt { get; set; }
}
