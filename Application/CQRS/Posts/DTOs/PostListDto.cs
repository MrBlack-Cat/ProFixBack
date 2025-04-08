namespace Application.CQRS.Posts.DTOs;

public class PostListDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public int ServiceProviderProfileId { get; set; }
    public string Content { get; set; } = null!;

    public string? ImageUrl { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
}
