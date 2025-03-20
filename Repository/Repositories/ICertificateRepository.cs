using Domain.Entities;
using Repository.Common;

namespace Repository.Repositories
{
    public interface ICertificateRepository : IRepository<Certificate>
    {
        Task<IEnumerable<Certificate>> GetByServiceProviderIdAsync(int serviceProviderProfileId);
    }
}
