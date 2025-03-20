namespace Application.CQRS.Messages.DTOs;

public class MessageListDto
{
    public int Id { get; set; }
    public string Content { get; set; } = null!;
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; }
}
