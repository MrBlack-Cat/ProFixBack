namespace Application.CQRS.GuaranteeDocuments.DTOs;

public class GuaranteeDocumentListDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string? FileUrl { get; set; }
    public DateTime CreatedAt { get; set; }
}

