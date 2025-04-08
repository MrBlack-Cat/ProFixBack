using Domain.Entities;

public interface IServiceTypeRepository
{
    Task<IEnumerable<ServiceType>> GetAllAsync();
    Task<ServiceType?> GetByIdAsync(int id);
    Task<IEnumerable<ServiceType>> GetByParentCategoryIdAsync(int parentCategoryId);
    Task<List<ServiceType>> SearchByNameAsync(string name);
}
