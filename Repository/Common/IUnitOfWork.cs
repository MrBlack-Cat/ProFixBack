using System.Threading.Tasks;
using Repository.Repositories;

namespace Repository.Common
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
  

        Task<int> SaveChangesAsync();
    }
}
