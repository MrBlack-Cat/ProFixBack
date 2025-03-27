namespace Application.CQRS.Complaints.DTOs;

public class UpdateComplaintDto
{
    public int Id { get; set; }
    public int TypeId { get; set; }
    public string? Description { get; set; }
    public string? UpdatedBy { get; set; }
}
