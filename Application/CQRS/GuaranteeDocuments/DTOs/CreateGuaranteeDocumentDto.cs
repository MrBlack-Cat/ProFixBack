namespace Application.CQRS.GuaranteeDocuments.DTOs;

public class CreateGuaranteeDocumentDto
{
    public int ClientProfileId { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public string? FileUrl { get; set; }
    public DateTime? IssueDate { get; set; }
    public DateTime? ExpirationDate { get; set; }
}
