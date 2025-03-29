namespace Application.CQRS.Complaints.DTOs;

public class DeleteComplaintDto
{
    public int Id { get; set; }
    public string? DeletedByUserId { get; set; }
    public string? Reason { get; set; }
}
