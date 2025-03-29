namespace Application.CQRS.ServiceProviderProfiles.DTOs;

public class ServiceProviderProfileListDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Surame { get; set; } = null!;
    public string? City { get; set; }
    public int? ExperienceYears { get; set; }
    public bool IsApprovedByAdmin { get; set; }
}
