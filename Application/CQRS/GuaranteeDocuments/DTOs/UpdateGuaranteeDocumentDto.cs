namespace Application.CQRS.GuaranteeDocuments.DTOs;

public class UpdateGuaranteeDocumentDto
{
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public string? FileUrl { get; set; }
}
