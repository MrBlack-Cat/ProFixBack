namespace Application.CQRS.ServiceProviderProfiles.DTOs;

public class ServiceProviderProfileListDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public string? City { get; set; }
    public int? ExperienceYears { get; set; }
    public string? AvatarUrl { get; set; }
    public double Rating { get; set; }
    public List<string> ServiceTypes { get; set; } = new();
}
