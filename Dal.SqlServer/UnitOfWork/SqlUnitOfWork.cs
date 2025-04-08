using Dal.SqlServer.Infrastructure;
using Dal.SqlServer.Infrastructure.TokenSecurity;
using DAL.SqlServer.Context;
using DAL.SqlServer.Infrastructure;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Repository.Common;
using Repository.Repositories;
using Repository.Repositories.TokenSecurity;
using System.Data;

namespace DAL.SqlServer.UnitOfWork;

public class SqlUnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private readonly IDbConnection _dbConnection;

    public SqlUnitOfWork(AppDbContext context, IConfiguration configuration)
    {
        _context = context;
        _dbConnection = new SqlConnection(configuration.GetConnectionString("myconn"));
        //_dbConnection.Open(); 
    }

    // Repositories
    private IUserRepository _userRepository;
    private IClientProfileRepository _clientProfileRepository;
    private IServiceProviderProfileRepository _serviceProviderProfileRepository;
    private ICertificateRepository _certificateRepository;
    private IPostRepository _postRepository;
    private IReviewRepository _reviewRepository;
    private IUserRoleRepository _userRoleRepository;
    private IActivityLogRepository _activityLogRepository;
    private IGuaranteeDocumentRepository _guaranteeDocumentRepository;
    private IMessageRepository _messageRepository;
    private INotificationRepository _notificationRepository;
    private IPortfolioItemRepository _portfolioItemRepository;
    private IRefreshTokenRepository _refreshTokenRepository;
    private IServiceTypeRepository _serviceTypeRepository;
    private IParentCategoryRepository _parentCategoryRepository;
    private IServiceProviderServiceTypeRepository serviceProviderServiceTypeRepository;





    public IClientProfileRepository ClientProfileRepository => _clientProfileRepository ??= new SqlClientProfileRepository(_dbConnection);

    public IServiceProviderProfileRepository ServiceProviderProfileRepository =>_serviceProviderProfileRepository ??= new SqlServiceProviderProfileRepository(_dbConnection);

    public ICertificateRepository CertificateRepository =>_certificateRepository ??= new SqlCertificateRepository(_dbConnection);

    //_____________________________________________________________________________________________________________________

    public IPostRepository PostRepository =>_postRepository ??= new SqlPostRepository(_dbConnection);

    public IReviewRepository ReviewRepository =>_reviewRepository ??= new SqlReviewRepository(_dbConnection);

    public IServiceBookingRepository ServiceBookingRepository => new SqlServiceBookingRepository(_dbConnection);

    public ISubscriptionPlanRepository SubscriptionPlanRepository => new SqlSubscriptionPlanRepository(_dbConnection);

    public ISupportTicketRepository SupportTicketRepository => new SqlSupportTicketRepository(_dbConnection);

    public IUserRepository UserRepository => _userRepository ??= new SqlUserRepository(_dbConnection);
    public IPortfolioItemRepository PortfolioItemRepository => _portfolioItemRepository ??= new SqlPortfolioItemRepository(_dbConnection);
    public IServiceTypeRepository ServiceTypeRepository => _serviceTypeRepository ??= new SqlServiceTypeRepository(_dbConnection);



    //______________________________________________________________________________________________________________________

    public IRefreshTokenRepository RefreshTokenRepository => _refreshTokenRepository ??= new SqlRefreshTokenRepository(_context);
    public IUserRoleRepository UserRoleRepository => _userRoleRepository ??= new SqlUserRoleRepository(_dbConnection);
    public IActivityLogRepository ActivityLogRepository => _activityLogRepository ??= new SqlActivityLogRepository(_dbConnection);
    public IGuaranteeDocumentRepository GuaranteeDocumentRepository => _guaranteeDocumentRepository ??= new SqlGuaranteeDocumentRepository(_dbConnection);
    public IMessageRepository MessageRepository => _messageRepository ??= new SqlMessageRepository(_dbConnection);
    public INotificationRepository NotificationRepository => _notificationRepository ??= new SqlNotificationRepository(_dbConnection);
    public IParentCategoryRepository ParentCategoryRepository => _parentCategoryRepository ??= new SqlParentCategoryRepository(_dbConnection);
    public IServiceProviderServiceTypeRepository ServiceProviderServiceTypeRepository => serviceProviderServiceTypeRepository ??= new SqlServiceProviderServiceTypeRepository(_dbConnection);
    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}
