using Microsoft.Extensions.DependencyInjection;
using Repository.Repositories;
using Dal.SqlServer.Infrastructure;
using DAL.SqlServer.Infrastructure;

namespace Infrastructure.DependencyInjection
{
    public static class RepositoryDependencyInjection
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IServiceProviderProfileRepository, SqlServiceProviderProfileRepository>();
            services.AddScoped<IClientProfileRepository, SqlClientProfileRepository>();
            services.AddScoped<IReviewRepository, SqlReviewRepository>();
            services.AddScoped<IPostRepository, SqlPostRepository>();
            services.AddScoped<IServiceBookingRepository, SqlServiceBookingRepository>();
            services.AddScoped<INotificationRepository, SqlNotificationRepository>();
            services.AddScoped<IPostLikeRepository, SqlPostLikeRepository>();
            services.AddScoped<IChatMessageRepository, SqlChatMessageRepository>();
            services.AddScoped<ICertificateRepository, SqlCertificateRepository>();
            services.AddScoped<IComplaintRepository, SqlComplaintRepository>();
            services.AddScoped<IGuaranteeDocumentRepository, SqlGuaranteeDocumentRepository>();
            services.AddScoped<IMessageRepository, SqlMessageRepository>();
            services.AddScoped<IParentCategoryRepository, SqlParentCategoryRepository>();
            services.AddScoped<IServiceProviderServiceTypeRepository, SqlServiceProviderServiceTypeRepository>();
            services.AddScoped<IServiceTypeRepository, SqlServiceTypeRepository>();
            services.AddScoped<IUserRepository, SqlUserRepository>();

            return services;
        }
    }
}
