namespace Application.CQRS.Users.DTOs;

public class RegisterUserDto
{
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public int UserRoleId { get; set; }
    public string? PhoneNumber { get; set; }
    public string? CreatedBy { get; set; }
}
