namespace Application.CQRS.Messages.DTOs;

public class CreateMessageDto
{
    public int ReceiverUserId { get; set; }
    public string Content { get; set; } = null!;
    public bool IsRead { get; set; } = false;
}
