namespace Application.CQRS.Posts.DTOs;

public class CreatePostDto
{
    public int Id { get; set; }
    public int ServiceProviderProfileId { get; set; }
    public string Title { get; set; } = null!;
    public string? Content { get; set; }
    public string? ImageUrl { get; set; }
    public string? CreatedBy { get; set; }  //yox IsDelete Base-den gelir axi
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow; //test ucun qoydum 
}
