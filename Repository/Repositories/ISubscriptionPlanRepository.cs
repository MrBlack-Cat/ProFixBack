using Domain.Entities;
using Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public interface ISubscriptionPlanRepository : IRepository<SubscriptionPlan>
    {
        Task<IEnumerable<SubscriptionPlan>> GetByServiceProviderIdAsync(int serviceProviderProfileId);
    }
}
