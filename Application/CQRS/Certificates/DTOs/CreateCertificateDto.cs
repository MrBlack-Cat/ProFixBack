namespace Application.CQRS.Certificates.DTOs;

public class CreateCertificateDto
{
    public int ServiceProviderProfileId { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public string? FileUrl { get; set; }
    public DateTime? IssuedAt { get; set; }
    public string? CreatedBy { get; set; }
}
