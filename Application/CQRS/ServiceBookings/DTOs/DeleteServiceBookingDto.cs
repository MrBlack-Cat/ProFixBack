namespace Application.CQRS.ServiceBookings.DTOs;

public class DeleteServiceBookingDto
{
    public int Id { get; set; }
    public string? DeletedByUserId { get; set; }
    public string? Reason { get; set; }
}
