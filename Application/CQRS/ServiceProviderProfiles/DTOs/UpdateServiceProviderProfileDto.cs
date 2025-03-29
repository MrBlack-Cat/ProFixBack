namespace Application.CQRS.ServiceProviderProfiles.DTOs;

public class UpdateServiceProviderProfileDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public string? City { get; set; }
    public int? Age { get; set; }
    public int? GenderId { get; set; }
    public int? ExperienceYears { get; set; }
    public string? Description { get; set; }
    public bool IsApprovedByAdmin { get; set; }
    public DateTime? ApprovalDate { get; set; }
    public int? UpdatedBy { get; set; }
}
