namespace Application.CQRS.PortfolioItems.DTOs;

public class CreatePortfolioItemDto
{
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
}
