namespace Application.CQRS.Reviews.DTOs
{
    public class ReviewListWithClientDto
    {
        public int Id { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }

        public int ClientProfileId { get; set; }
        public string? ClientName { get; set; }
        public string? ClientAvatarUrl { get; set; }
    }
}
