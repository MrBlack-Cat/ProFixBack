using System.Threading.Tasks;
using Repository.Repositories;
using Repository.Repositories.TokenSecurity;

namespace Repository.Common
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }

        public IRefreshTokenRepository RefreshTokenRepository { get; }
        public IPostRepository PostRepository { get; }
        public IServiceProviderProfileRepository ServiceProviderProfileRepository { get; }
        public IReviewRepository ReviewRepository { get; }
        public IServiceBookingRepository ServiceBookingRepository { get; }
        public ISubscriptionPlanRepository SubscriptionPlanRepository { get; }  
        public ISupportTicketRepository SupportTicketRepository { get; }
        public IActivityLogRepository ActivityLogRepository { get; }
        public IClientProfileRepository ClientProfileRepository { get; }
        public IGuaranteeDocumentRepository GuaranteeDocumentRepository { get; }
        public ICertificateRepository CertificateRepository { get; }
        public IMessageRepository MessageRepository { get; }
        public INotificationRepository NotificationRepository { get; }
        public IUserRoleRepository UserRoleRepository { get; }
        public IPortfolioItemRepository PortfolioItemRepository { get; }
        public IServiceTypeRepository ServiceTypeRepository { get; }
        public IParentCategoryRepository ParentCategoryRepository { get; }
        IServiceProviderServiceTypeRepository ServiceProviderServiceTypeRepository { get; }
        IComplaintRepository ComplaintRepository { get; }
        public IComplaintTypeRepository ComplaintTypeRepository { get; }
        public IChatMessageRepository ChatMessageRepository { get; }
        public IPostLikeRepository PostLikeRepository { get; }







        Task<int> SaveChangesAsync();
    }
}
