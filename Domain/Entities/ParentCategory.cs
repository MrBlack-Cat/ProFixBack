namespace Domain.Entities;

public class ParentCategory
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public ICollection<ServiceType> ServiceTypes { get; set; } = new List<ServiceType>();
}
