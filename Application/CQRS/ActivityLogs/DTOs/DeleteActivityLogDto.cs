namespace Application.CQRS.ActivityLogs.DTOs;

public class DeleteActivityLogDto
{
    public int Id { get; set; }
    public string? DeletedByUserId { get; set; }
    public string? Reason { get; set; }
}
