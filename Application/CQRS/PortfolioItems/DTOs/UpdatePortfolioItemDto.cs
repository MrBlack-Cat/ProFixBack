namespace Application.CQRS.PortfolioItems.DTOs;

public class UpdatePortfolioItemDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public string? UpdatedBy { get; set; }
}
