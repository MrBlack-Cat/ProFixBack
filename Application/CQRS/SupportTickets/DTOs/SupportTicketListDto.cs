namespace Application.CQRS.SupportTickets.DTOs;

public class SupportTicketListDto
{
    public int Id { get; set; }
    public string Subject { get; set; } = null!;
    public int StatusId { get; set; }
    public DateTime CreatedAt { get; set; }
}
