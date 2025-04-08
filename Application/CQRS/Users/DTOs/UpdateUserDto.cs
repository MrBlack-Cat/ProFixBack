namespace Application.CQRS.Users.DTOs;

public class UpdateUserDto
{
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? PhoneNumber { get; set; }
}
