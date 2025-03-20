namespace Application.CQRS.Certificates.DTOs;

public class DeleteCertificateDto
{
    public int Id { get; set; }
    public string? DeletedByUserId { get; set; }
    public string? Reason { get; set; }
}
