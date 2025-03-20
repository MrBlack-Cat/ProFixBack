namespace Application.CQRS.Posts.DTOs;

public class PostListDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string? ImageUrl { get; set; }
    public DateTime CreatedAt { get; set; }
}
