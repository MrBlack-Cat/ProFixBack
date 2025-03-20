namespace Application.CQRS.PortfolioItems.DTOs;

public class DeletePortfolioItemDto
{
    public int Id { get; set; }
    public string? DeletedByUserId { get; set; }
    public string? Reason { get; set; }
}
