namespace Application.CQRS.GuaranteeDocuments.DTOs;

public class UpdateGuaranteeDocumentDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public string? FileUrl { get; set; }
    public string? UpdatedBy { get; set; }
}
