namespace Domain.Entities;

public class ServiceType
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public int ParentCategoryId { get; set; }
    public ParentCategory? ParentCategory { get; set; }
}
