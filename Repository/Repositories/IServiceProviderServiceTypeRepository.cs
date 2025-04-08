using Domain.Entities;
using Domain.Types;

namespace Repository.Repositories;

public interface IServiceProviderServiceTypeRepository
{
    Task<List<ServiceProviderServiceType>> GetByServiceProviderProfileIdAsync(int profileId);

    Task AddAsync(ServiceProviderServiceType entity);

    Task AddRangeAsync(List<ServiceProviderServiceType> entities);

    Task DeleteAllByServiceProviderProfileIdAsync(int profileId);
}
