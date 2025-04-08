namespace Application.CQRS.Reviews.DTOs;

public class DeleteReviewDto
{
    public int Id { get; set; }
    public string? Reason { get; set; }
}
