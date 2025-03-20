namespace Application.CQRS.ServiceBookings.DTOs;

public class ServiceBookingListDto
{
    public int Id { get; set; }
    public int ClientProfileId { get; set; }
    public int ServiceProviderProfileId { get; set; }
    public int StatusId { get; set; }
    public DateTime? ScheduledDate { get; set; }
}
