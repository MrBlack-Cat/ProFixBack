namespace Application.CQRS.ClientProfiles.DTOs;

public class ClientProfileListDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = null!;
    public string? City { get; set; }
    public string? AvatarUrl { get; set; }
}
