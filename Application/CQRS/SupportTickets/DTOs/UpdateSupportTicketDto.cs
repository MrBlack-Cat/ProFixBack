namespace Application.CQRS.SupportTickets.DTOs;

public class UpdateSupportTicketDto
{
    public int Id { get; set; }
    public string Subject { get; set; } = null!;
    public string Message { get; set; } = null!;
    public int StatusId { get; set; }
    public int? UpdatedBy { get; set; }
}
