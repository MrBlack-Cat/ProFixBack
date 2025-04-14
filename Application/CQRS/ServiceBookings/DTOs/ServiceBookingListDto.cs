namespace Application.CQRS.ServiceBookings.DTOs;

public class ServiceBookingListDto
{
    public int Id { get; set; }
    public string? Description { get; set; }
    public DateTime ScheduledDate { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }

    public string Status { get; set; } = null!;
    public bool IsConfirmedByProvider { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime? ConfirmationDate { get; set; }
    public DateTime? PendingDate { get; set; }
    public DateTime? RejectedDate { get; set; }
    public DateTime? CancelledDate { get; set; }
    public DateTime? InProgressDate { get; set; }
    public DateTime? CompletionDate { get; set; }
    public DateTime? CreatedAt { get; set; }
    public string? ServiceProviderName { get; set; }
    public int? ServiceProviderProfileId { get; set; }
    public string? ServiceProviderAvatarUrl { get; set; }
    public int? ClientProfileId { get; set; }
    public string? ClientName { get; set; }
    public string? ClientAvatarUrl { get; set; }

}
