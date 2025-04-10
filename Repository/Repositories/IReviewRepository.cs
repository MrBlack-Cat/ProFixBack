using Domain.Entities;
using Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public interface IReviewRepository : IRepository<Review>
    {
        Task<IEnumerable<Review>> GetByServiceProviderIdAsync(int serviceProviderProfileId);
        Task<IEnumerable<Review>> GetByClientIdAsync(int clientProfileId);
        Task<double> GetAverageRatingByProviderIdAsync(int providerId);
        Task<Review?> GetByClientAndProviderAsync(int clientId, int providerId);


    }
}
