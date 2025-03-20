namespace Application.CQRS.ClientProfiles.DTOs;

public class DeleteClientProfileDto
{
    public int Id { get; set; }
    public string? DeletedByUserId { get; set; }
    public string? Reason { get; set; }
}
