namespace Application.CQRS.ClientProfiles.DTOs;

public class ClientProfileDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public string? Bio { get; set; }
    public string? Country { get; set; }
    public DateTime CreatedAt { get; set; }
}
