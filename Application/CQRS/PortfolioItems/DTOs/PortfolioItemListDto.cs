namespace Application.CQRS.PortfolioItems.DTOs;

public class PortfolioItemListDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string? ImageUrl { get; set; }
}
