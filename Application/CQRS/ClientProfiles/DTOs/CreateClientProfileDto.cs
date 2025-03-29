namespace Application.CQRS.ClientProfiles.DTOs;

public class CreateClientProfileDto
{
    public int UserId { get; set; }
    public string Surname { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? City { get; set; }
    public string? AvatarUrl { get; set; }
    public string? About { get; set; }
    public string? OtherContactLinks { get; set; }
    public string? CreatedBy { get; set; }
}
