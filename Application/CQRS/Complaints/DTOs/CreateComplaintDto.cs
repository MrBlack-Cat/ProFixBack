namespace Application.CQRS.Complaints.DTOs;

public class CreateComplaintDto
{
    public int ToUserId { get; set; }
    public int TypeId { get; set; }
    public string? Description { get; set; }
}
