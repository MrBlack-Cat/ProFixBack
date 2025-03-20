namespace Application.CQRS.Users.DTOs;

public class DeleteUserDto
{
    public int Id { get; set; }
    public string? DeletedByUserId { get; set; }
    public string? Reason { get; set; }
}
