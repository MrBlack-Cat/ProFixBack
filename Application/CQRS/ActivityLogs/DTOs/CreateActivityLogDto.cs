namespace Application.CQRS.ActivityLogs.DTOs;

public class CreateActivityLogDto
{
    public int UserId { get; set; }
    public string Action { get; set; } = null!;
    public string EntityType { get; set; } = null!;
    public int EntityId { get; set; }
    public string? CreatedBy { get; set; }
}
