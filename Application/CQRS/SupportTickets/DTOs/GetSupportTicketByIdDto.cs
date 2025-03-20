namespace Application.CQRS.SupportTickets.DTOs;

public class GetSupportTicketByIdDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Subject { get; set; } = null!;
    public string Message { get; set; } = null!;
    public int StatusId { get; set; }
    public DateTime CreatedAt { get; set; }
}
