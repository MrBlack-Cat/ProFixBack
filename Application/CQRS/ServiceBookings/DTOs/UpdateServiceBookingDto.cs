namespace Application.CQRS.ServiceBookings.DTOs;

public class UpdateServiceBookingDto
{
    public string? Description { get; set; }
    public DateTime? ScheduledDate { get; set; }
    public TimeSpan? StartTime { get; set; }
    public TimeSpan? EndTime { get; set; }
}
