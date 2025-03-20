namespace Application.CQRS.ServiceProviderProfiles.DTOs;

public class ServiceProviderProfileListDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = null!;
    public string? City { get; set; }
    public int? ExperienceYears { get; set; }
    public bool IsApprovedByAdmin { get; set; }
}
