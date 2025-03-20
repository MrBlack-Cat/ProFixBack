namespace Application.CQRS.Messages.DTOs;

public class CreateMessageDto
{
    public int SenderUserId { get; set; }
    public int ReceiverUserId { get; set; }
    public string Content { get; set; } = null!;
    public bool IsRead { get; set; } = false;
    public string? CreatedBy { get; set; }
}
