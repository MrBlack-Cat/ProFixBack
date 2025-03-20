namespace Application.CQRS.SubscriptionPlans.DTOs;

public class GetSubscriptionPlanByIdDto
{
    public int Id { get; set; }
    public int ServiceProviderProfileId { get; set; }
    public string PlanName { get; set; } = null!;
    public decimal Price { get; set; }
    public int DurationInDays { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime CreatedAt { get; set; }
}
