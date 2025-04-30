namespace Application.CQRS.ServiceProviderProfiles.DTOs;

public class ServiceProviderProfileListDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public string? City { get; set; }
    public int? Age { get; set; }
    public int? ExperienceYears { get; set; }
    public string? AvatarUrl { get; set; }
    public double Rating { get; set; }
    public string? GenderName { get; set; }
    public DateTime? ApprovalDate { get; set; }
    public int? ParentCategoryId { get; set; }
    public string? ParentCategoryName { get; set; }
    public List<int> ServiceTypeIds { get; set; } = new();
    public List<string> ServiceTypes { get; set; } = new();
}
