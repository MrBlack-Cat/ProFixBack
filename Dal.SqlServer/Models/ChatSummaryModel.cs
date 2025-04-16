namespace Dal.SqlServer.Models;

public class ChatSummaryModel
{
    public int OtherUserId { get; set; }
    public string OtherUserName { get; set; } = null!;
    public string? OtherUserAvatarUrl { get; set; }
    public string LastMessageContent { get; set; } = null!;
    public DateTime LastMessageTime { get; set; }
}
