namespace Application.CQRS.Certificates.DTOs;

public class CertificateListDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public string? FileUrl { get; set; }
    public DateTime? IssuedAt { get; set; }
}
