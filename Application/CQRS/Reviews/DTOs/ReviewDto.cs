namespace Application.CQRS.Reviews.DTOs;

public class ReviewDto
{
    public int ClientProfileId { get; set; }
    public int ServiceProviderProfileId { get; set; }
    public int Rating { get; set; }
    public string Comment { get; set; }
    public string ClientName { get; set; }
    public string ClientAvatarUrl { get; set; }
}
