using Domain.BaseEntities;
using Domain.Entities;
using Domain.Other;

public class ServiceBooking : BaseEntity
{
    public int ClientProfileId { get; set; }
    public string? ClientName { get; set; }
    public string? ClientAvatarUrl { get; set; }
    public int ServiceProviderProfileId { get; set; }
    public string? ServiceProviderName { get; set; }
    public string? ServiceProviderAvatarUrl { get; set; }
    public string? Description { get; set; }
    public int StatusId { get; set; }
    public string Status { get; set; } = null!;
    public DateTime ScheduledDate { get; set; }
    public TimeSpan StartTime { get; set; } 
    public TimeSpan EndTime { get; set; }   

    public bool IsConfirmedByProvider { get; set; }
    public DateTime? ConfirmationDate { get; set; }

    public bool IsCompleted { get; set; }
    public DateTime? CompletionDate { get; set; }

    public ClientProfile? ClientProfile { get; set; }
    public ServiceProviderProfile? ServiceProviderProfile { get; set; }
    //public ServiceBookingStatus? Status { get; set; }
    public DateTime? PendingDate { get; set; }
    public DateTime? RejectedDate { get; set; }
    public DateTime? CancelledDate { get; set; }
    public DateTime? InProgressDate { get; set; }

}
