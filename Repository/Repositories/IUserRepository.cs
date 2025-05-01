using Domain.Entities;
using Repository.Common;

namespace Repository.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetByEmailAsync(string email);
        Task<User> GetByUserNameAsync(string username);
        Task RegisterAsync (User user); //yeni elave eledim 
        Task DeleteAsync(int id, string deletedReason, int deletedBy);
        Task<IEnumerable<User>> GetUsersByRoleAsync(int roleId);



    }
}
