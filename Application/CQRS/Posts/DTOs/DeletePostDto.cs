namespace Application.CQRS.Posts.DTOs;

public class DeletePostDto
{
    public int Id { get; set; }
    public string? DeletedByUserId { get; set; }
    public string? Reason { get; set; }
}
