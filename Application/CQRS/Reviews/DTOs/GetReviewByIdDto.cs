namespace Application.CQRS.Reviews.DTOs;

public class GetReviewByIdDto
{
    public int Id { get; set; }
    public int ClientProfileId { get; set; }
    public int ServiceProviderProfileId { get; set; }
    public int Rating { get; set; }
    public string? Comment { get; set; }
    public DateTime CreatedAt { get; set; }
}
