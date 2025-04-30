using Domain.Entities;
using Repository.Common;

namespace Repository.Repositories
{
    public interface ICertificateRepository : IRepository<Certificate>
    {
        Task<IEnumerable<Certificate>> GetByServiceProviderIdAsync(int serviceProviderProfileId);
        Task<IEnumerable<Certificate>> GetByUserIdAsync(int userId);
        Task<IEnumerable<Certificate>> GetAllCertificatesAsync();

    }
}
