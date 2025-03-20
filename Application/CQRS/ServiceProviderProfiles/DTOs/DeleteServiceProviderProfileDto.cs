namespace Application.CQRS.ServiceProviderProfiles.DTOs;

public class DeleteServiceProviderProfileDto
{
    public int Id { get; set; }
    public string? DeletedByUserId { get; set; }
    public string? Reason { get; set; }
}
