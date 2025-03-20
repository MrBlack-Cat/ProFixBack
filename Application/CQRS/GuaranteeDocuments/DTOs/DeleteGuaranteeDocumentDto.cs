namespace Application.CQRS.GuaranteeDocuments.DTOs;

public class DeleteGuaranteeDocumentDto
{
    public int Id { get; set; }
    public string? DeletedByUserId { get; set; }
    public string? Reason { get; set; }
}
