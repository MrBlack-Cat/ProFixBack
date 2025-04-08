namespace Application.CQRS.Notifications.DTOs;

public class UpdateNotificationDto
{
    public int Id { get; set; }
    public int TypeId { get; set; }
    public string Message { get; set; } = null!;
    public bool IsRead { get; set; }
}
