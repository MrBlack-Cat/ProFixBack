using Domain.Entities;


namespace Repository.Repositories;


public interface IParentCategoryRepository
{
    Task<IEnumerable<ParentCategory>> GetAllAsync();
    Task<ParentCategory?> GetByIdAsync(int id);
    Task<ParentCategory?> GetByNameAsync(string name);

}
