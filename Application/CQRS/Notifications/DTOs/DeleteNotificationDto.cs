namespace Application.CQRS.Notifications.DTOs;

public class DeleteNotificationDto
{
    public int Id { get; set; }
    public string? DeletedByUserId { get; set; }
    public string? Reason { get; set; }
}
