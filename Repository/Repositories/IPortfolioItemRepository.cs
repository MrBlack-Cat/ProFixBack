using Domain.Entities;
using Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public interface IPortfolioItemRepository : IRepository<PortfolioItem>
    {
        Task<IEnumerable<PortfolioItem>> GetByServiceProviderIdAsync(int serviceProviderProfileId);
    }
}
