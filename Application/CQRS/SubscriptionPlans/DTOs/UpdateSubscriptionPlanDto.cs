namespace Application.CQRS.SubscriptionPlans.DTOs;

public class UpdateSubscriptionPlanDto
{
    public int Id { get; set; }
    public string PlanName { get; set; } = null!;
    public decimal Price { get; set; }
    public int DurationInDays { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int? UpdatedBy { get; set; }
}
