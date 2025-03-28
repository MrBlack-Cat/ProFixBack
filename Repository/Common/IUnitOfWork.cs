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
        IGuaranteeDocumentRepository GuaranteeDocumentRepository { get; }
        ICertificateRepository CertificateRepository { get; }
        IMessageRepository MessageRepository { get; }
        INotificationRepository NotificationRepository { get; }


        Task<int> SaveChangesAsync();
    }
}
