using System.Security.Claims;

namespace Repository.Common
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id , ClaimsPrincipal user);            
        Task DeleteAsync(T entity);           
    }
}



