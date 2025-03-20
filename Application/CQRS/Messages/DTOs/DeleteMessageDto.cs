namespace Application.CQRS.Messages.DTOs;

public class DeleteMessageDto
{
    public int Id { get; set; }
    public string? DeletedByUserId { get; set; }
    public string? Reason { get; set; }
}
