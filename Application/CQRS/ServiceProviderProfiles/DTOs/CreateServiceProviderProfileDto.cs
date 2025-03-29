namespace Application.CQRS.ServiceProviderProfiles.DTOs;

public class CreateServiceProviderProfileDto
{
    public int UserId { get; set; }
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public string? City { get; set; }
    public int? Age { get; set; }
    public int? GenderId { get; set; }
    public int? ExperienceYears { get; set; }
    public string? Description { get; set; }
    public string? CreatedBy { get; set; }
}
