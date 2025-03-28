using Application.Common.Behaviors;
using Application.Common.Interfaces;
using Application.Services;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Infrastructure.Behaviors;
using Infrastructure.Services;
using Application.Mappings;
using Common.Interfaces;

namespace Application.DependencyInjection
{
    public static class ApplicationDependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Application core
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddAutoMapper(typeof(ActivityLogProfile).Assembly);
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            // Common services
            services.AddScoped<IUserContext, UserContext>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<ILoggerService, LoggerService>();
            services.AddScoped<IAuthorizationService, AuthorizationService>();
            services.AddScoped<IActivityLoggerService, ActivityLoggerService>();
            services.AddScoped<ICloudStorageService, GoogleCloudStorageService>();



            //services.AddScoped<ITokenService, TokenService>();

            // Validation pipeline
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ExceptionHandlingBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehavior<,>));


            // Infrastructure
            services.AddHttpContextAccessor();

            return services;
        }
    }
}
