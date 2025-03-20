namespace Application.CQRS.Posts.DTOs;

public class GetPostByIdDto
{
    public int Id { get; set; }
    public int ServiceProviderProfileId { get; set; }
    public string Title { get; set; } = null!;
    public string? Content { get; set; }
    public string? ImageUrl { get; set; }
    public DateTime CreatedAt { get; set; }
}
