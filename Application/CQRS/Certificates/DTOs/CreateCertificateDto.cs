public class CreateCertificateDto
{
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public string? FileUrl { get; set; }
    public DateTime? IssuedAt { get; set; }
}
