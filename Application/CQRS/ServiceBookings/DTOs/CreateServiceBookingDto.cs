namespace Application.CQRS.ServiceBookings.DTOs;

public class CreateServiceBookingDto
{
    public int ServiceProviderProfileId { get; set; }
    public string? Description { get; set; }

    public DateTime ScheduledDate { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
}
