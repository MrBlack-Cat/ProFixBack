namespace Application.CQRS.Users.DTOs;

public class GetAllUserDto
{
    public int Id { get; set; }
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public bool IsActive { get; set; }
    public string? RoleName { get; set; }
}
