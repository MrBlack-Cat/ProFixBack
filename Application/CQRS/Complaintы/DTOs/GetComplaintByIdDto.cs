namespace Application.CQRS.Complaints.DTOs;

public class GetComplaintByIdDto
{
    public int Id { get; set; }
    public int FromUserId { get; set; }
    public int ToUserId { get; set; }
    public int TypeId { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
}
