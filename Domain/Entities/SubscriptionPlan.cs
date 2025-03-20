using Domain.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class SubscriptionPlan : BaseEntity
    {
        public int ServiceProviderProfileId { get; set; }
        public string PlanName { get; set; } = null!;
        public decimal Price { get; set; }
        public int DurationInDays { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public ServiceProviderProfile? ServiceProviderProfile { get; set; }
    }
}
