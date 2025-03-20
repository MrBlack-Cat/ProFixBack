namespace Application.CQRS.PortfolioItems.DTOs;

public class CreatePortfolioItemDto
{
    public int ServiceProviderProfileId { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public string? CreatedBy { get; set; }
}
