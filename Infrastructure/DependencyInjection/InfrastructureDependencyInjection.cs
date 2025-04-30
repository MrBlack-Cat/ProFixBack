using Common.Interfaces;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using Repository.Repositories;
using Dal.SqlServer.Infrastructure;
using Application.Common.Interfaces;
using Application.Services;
using DAL.SqlServer.Infrastructure;

namespace Infrastructure.DependencyInjection
{
    public static class InfrastructureDependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            //  HttpContext 
            services.AddScoped<IUserContext, UserContext>();

            //  Services 
            services.AddScoped<ICloudStorageService, GoogleCloudStorageService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddHttpClient<IChatService, ChatService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<ILoggerService, LoggerService>();
            services.AddScoped<IActivityLoggerService, ActivityLoggerService>();
            services.AddScoped<IAuthorizationService, AuthorizationService>();



            //  Repositories 
            services.AddRepositories();


            return services;
        }
    }
}
