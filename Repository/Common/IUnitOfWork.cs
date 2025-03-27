using System.Threading.Tasks;
using Repository.Repositories;
using Repository.Repositories.TokenSecurity;

namespace Repository.Common
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        //yeni elave 
        public IRefreshTokenRepository RefreshTokenRepository { get; }
        public IPostRepository PostRepository { get; }
        public IServiceProviderProfileRepository ServiceProviderProfileRepository { get; }


        Task<int> SaveChangesAsync();
    }
}
