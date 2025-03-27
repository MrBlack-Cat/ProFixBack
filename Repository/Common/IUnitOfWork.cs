using System.Threading.Tasks;
using Repository.Repositories;
using Repository.Repositories.TokenSecurity;

namespace Repository.Common
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        //yeni elave 
        IRefreshTokenRepository RefreshTokenRepository { get; }
        IClientProfileRepository ClientProfileRepository { get; }
        IUserRoleRepository UserRoleRepository { get; }
        IActivityLogRepository ActivityLogRepository { get; }


        Task<int> SaveChangesAsync();
    }
}
