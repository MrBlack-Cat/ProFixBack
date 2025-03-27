using System.Threading.Tasks;
using Repository.Repositories;
using Repository.Repositories.TokenSecurity;

namespace Repository.Common
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }

        IRefreshTokenRepository RefreshTokenRepository { get; }
        IPostRepository PostRepository { get; }
        IServiceProviderProfileRepository ServiceProviderProfileRepository { get; }
        IClientProfileRepository ClientProfileRepository { get; }
        IUserRoleRepository UserRoleRepository { get; }
        IActivityLogRepository ActivityLogRepository { get; }

        Task<int> SaveChangesAsync();
    }
}
