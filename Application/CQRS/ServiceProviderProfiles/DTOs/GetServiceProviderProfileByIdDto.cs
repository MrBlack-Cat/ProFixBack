namespace Application.CQRS.ServiceProviderProfiles.DTOs;

public class GetServiceProviderProfileByIdDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public string? City { get; set; }
    public int? Age { get; set; }
    public string? GenderName { get; set; }
    public int? ExperienceYears { get; set; }
    public string? Description { get; set; }
    public string? AvatarUrl { get; set; }            
    public string? ParentCategoryName { get; set; }   
    public List<string> ServiceTypes { get; set; } = new(); 
    public bool IsApprovedByAdmin { get; set; }
    public DateTime? ApprovalDate { get; set; }
    public DateTime CreatedAt { get; set; }
}
