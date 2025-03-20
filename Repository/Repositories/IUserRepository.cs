using Domain.Entities;
using Repository.Common;

namespace Repository.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetByEmailAsync(string email);
        Task<User> GetByUserNameAsync(string username);
    }
}
