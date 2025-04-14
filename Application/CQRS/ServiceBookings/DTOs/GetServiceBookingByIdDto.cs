namespace Application.CQRS.ServiceBookings.DTOs;

public class GetServiceBookingByIdDto
{
    public int Id { get; set; }
    public int ClientProfileId { get; set; }
    public int ServiceProviderProfileId { get; set; }

    public string? Description { get; set; }

    public DateTime ScheduledDate { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }

    public string Status { get; set; } = null!;
    public bool IsConfirmedByProvider { get; set; }
    public DateTime? ConfirmationDate { get; set; }

    public bool IsCompleted { get; set; }
    public DateTime? CompletionDate { get; set; }
}
