namespace Application.CQRS.ClientProfiles.DTOs;

public class ClientProfileListDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public string? City { get; set; }
    public string? AvatarUrl { get; set; }
}
