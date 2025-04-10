namespace Application.CQRS.Reviews.DTOs;

public class CreateReviewCommandDto
{
    public int ServiceProviderProfileId { get; set; }
    public int Rating { get; set; }
    public string Comment { get; set; }
}
