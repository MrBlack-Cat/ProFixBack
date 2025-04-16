namespace Application.CQRS.Messages.DTOs;

public class ChatSummaryDto
{
    public int OtherUserId { get; set; }
    public string OtherUserName { get; set; } = null!;
    public string LastMessageContent { get; set; } = null!;
    public DateTime LastMessageTime { get; set; }
}
