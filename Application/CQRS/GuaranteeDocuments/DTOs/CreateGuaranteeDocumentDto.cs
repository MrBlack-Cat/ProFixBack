namespace Application.CQRS.GuaranteeDocuments.DTOs;

public class CreateGuaranteeDocumentDto
{
    public int ServiceProviderProfileId { get; set; }
    public int ClientProfileId { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public string? FileUrl { get; set; }
    public string? CreatedBy { get; set; }
}
