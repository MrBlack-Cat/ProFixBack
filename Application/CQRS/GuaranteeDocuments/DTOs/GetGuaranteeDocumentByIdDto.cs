namespace Application.CQRS.GuaranteeDocuments.DTOs;

public class GetGuaranteeDocumentByIdDto
{
    public int Id { get; set; }
    public int ServiceProviderProfileId { get; set; }
    public int ClientProfileId { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public string? FileUrl { get; set; }
    public DateTime CreatedAt { get; set; }
}
