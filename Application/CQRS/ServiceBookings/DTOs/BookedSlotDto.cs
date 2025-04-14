namespace Application.CQRS.ServiceBookings.Queries.GetBookedSlots;

public class BookedSlotDto
{
    public string StartTime { get; set; } = null!;
    public string EndTime { get; set; } = null!;
    public string Range => $"{StartTime:hh\\:mm} – {EndTime:hh\\:mm}";

}
