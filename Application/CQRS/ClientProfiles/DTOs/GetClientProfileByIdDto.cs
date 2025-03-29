namespace Application.CQRS.ClientProfiles.DTOs;

public class GetClientProfileByIdDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public string? City { get; set; }
    public string? AvatarUrl { get; set; }
    public string? About { get; set; }
    public string? OtherContactLinks { get; set; }
    public DateTime CreatedAt { get; set; }
}
