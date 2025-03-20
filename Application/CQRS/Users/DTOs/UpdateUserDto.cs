namespace Application.CQRS.Users.DTOs;

public class UpdateUserDto
{
    public int Id { get; set; }
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public int UserRoleId { get; set; }
    public string? PhoneNumber { get; set; }
    public string? UpdatedBy { get; set; }
}
