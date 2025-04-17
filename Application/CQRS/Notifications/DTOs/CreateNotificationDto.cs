namespace Application.CQRS.Notifications.DTOs;

public class CreateNotificationDto
{
    public int UserId { get; set; }
    public int TypeId { get; set; }
    public string Message { get; set; } = null!;
    public bool IsRead { get; set; } = false;
    public int CreatedBy { get; set; }
}
