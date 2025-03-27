namespace Application.CQRS.ServiceProviderProfiles.DTOs;

public class DeleteServiceProviderProfileDto
{
    public int Id { get; set; }
    public int? DeletedByUserId { get; set; }  //int etdim 
    public string? Reason { get; set; }
}
