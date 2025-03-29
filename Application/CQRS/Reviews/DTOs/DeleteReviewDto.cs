namespace Application.CQRS.Reviews.DTOs;

public class DeleteReviewDto
{
    public int Id { get; set; }
    public int? DeletedByUserId { get; set; } // int etdim string den 
    public string? Reason { get; set; }
}
