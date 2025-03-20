namespace Application.CQRS.PortfolioItems.DTOs;

public class GetPortfolioItemByIdDto
{
    public int Id { get; set; }
    public int ServiceProviderProfileId { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public DateTime CreatedAt { get; set; }
}
