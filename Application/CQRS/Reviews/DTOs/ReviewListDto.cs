namespace Application.CQRS.Reviews.DTOs;

public class ReviewListDto
{
    public int Id { get; set; }
    public int Rating { get; set; }
    public string? Comment { get; set; }
    public DateTime CreatedAt { get; set; }
}
