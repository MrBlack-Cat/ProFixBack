namespace Application.CQRS.SupportTickets.DTOs;

public class DeleteSupportTicketDto
{
    public int Id { get; set; }
    public string? DeletedByUserId { get; set; }
    public string? Reason { get; set; }
}
