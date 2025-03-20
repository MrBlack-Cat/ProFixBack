namespace Application.CQRS.Messages.DTOs;

public class UpdateMessageDto
{
    public int Id { get; set; }
    public string Content { get; set; } = null!;
    public bool IsRead { get; set; }
    public string? UpdatedBy { get; set; }
}
