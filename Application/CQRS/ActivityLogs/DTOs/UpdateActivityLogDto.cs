namespace Application.CQRS.ActivityLogs.DTOs;

public class UpdateActivityLogDto
{
    public int Id { get; set; }
    public string Action { get; set; } = null!;
    public string EntityType { get; set; } = null!;
    public int EntityId { get; set; }
    public string? UpdatedBy { get; set; }
}
