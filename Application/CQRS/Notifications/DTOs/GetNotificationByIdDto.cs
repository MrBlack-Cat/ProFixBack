namespace Application.CQRS.Notifications.DTOs;

public class GetNotificationByIdDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int TypeId { get; set; }
    public string Message { get; set; } = null!;
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; }
}
