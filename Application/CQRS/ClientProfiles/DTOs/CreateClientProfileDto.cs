namespace Application.CQRS.ClientProfiles.DTOs;

public class CreateClientProfileDto
{
    public int UserId { get; set; }
    public string FullName { get; set; } = null!;
    public string? City { get; set; }
    public string? AvatarUrl { get; set; }
    public string? About { get; set; }
    public string? OtherContactLinks { get; set; }
    public string? CreatedBy { get; set; }
}
