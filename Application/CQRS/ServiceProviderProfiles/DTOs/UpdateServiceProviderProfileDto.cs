namespace Application.CQRS.ServiceProviderProfiles.DTOs;

public class UpdateServiceProviderProfileDto
{
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public string? City { get; set; }
    public int? Age { get; set; }
    public int? GenderId { get; set; }
    public int? ExperienceYears { get; set; }
    //public string? Description { get; set; }
    public string? AvatarUrl { get; set; }
    public bool IsActive { get; set; }
    public DateTime? ApprovalDate { get; set; }
    public List<int>? ServiceTypeIds { get; set; }

}
