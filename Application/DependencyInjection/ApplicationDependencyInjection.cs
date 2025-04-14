using Application.Common.Behaviors;
using Application.Common.Interfaces;
using Application.CQRS.ClientProfiles.Queries.Handlers;
using Application.Mappings;
using Application.Services;
using Application.Validators.ServiceBookings;
using Common.Interfaces;
using Common.Options;
using Dal.SqlServer.Infrastructure;
using FluentValidation;
using Infrastructure.Behaviors;
using Infrastructure.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Repository.Repositories;
using System.Reflection;

namespace Application.DependencyInjection;

public static class ApplicationDependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {

        services.AddValidatorsFromAssembly(typeof(CreateServiceBookingCommandValidator).Assembly);


        services.AddOptions<ValidationOptions>()
            .BindConfiguration("Validation")
            .ValidateDataAnnotations();

        
        services.AddAutoMapper(
            Assembly.GetExecutingAssembly(),
            typeof(ActivityLogProfile).Assembly,
            typeof(MessageProfile).Assembly,
            typeof(PortfolioItemProfile).Assembly,
            typeof(ServiceBookingProfile).Assembly
        );

        //MediatR
        services.AddMediatR(Assembly.GetExecutingAssembly());
        services.AddMediatR(typeof(GetClientProfileByUserIdQueryHandler).Assembly);


        //FluentValidation
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        //(Pipeline)
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LocalizedValidationBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ExceptionHandlingBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehavior<,>));

        //Common/Infrastructure
        services.AddScoped<IUserContext, UserContext>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<ILoggerService, LoggerService>();
        services.AddScoped<IAuthorizationService, AuthorizationService>();
        services.AddScoped<IActivityLoggerService, ActivityLoggerService>();
        services.AddScoped<ICloudStorageService, GoogleCloudStorageService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IServiceProviderProfileRepository, SqlServiceProviderProfileRepository>();
        services.AddScoped<IPostRepository, SqlPostRepository>();
        services.AddScoped<IClientProfileRepository, SqlClientProfileRepository>();
        services.AddScoped<IReviewRepository, SqlReviewRepository>();
        services.AddScoped<IReviewRepository, SqlReviewRepository>();
        services.AddScoped<IServiceBookingRepository, SqlServiceBookingRepository>();






        // === HttpContext ===
        services.AddHttpContextAccessor();

        return services;
    }
}














//using Application.Common.Behaviors;
//using Application.Common.Interfaces;
//using Application.Services;
//using FluentValidation;
//using MediatR;
//using Microsoft.Extensions.DependencyInjection;
//using System.Reflection;
//using Infrastructure.Behaviors;
//using Infrastructure.Services;
//using Application.Mappings;
//using Common.Interfaces;
//using Application.Validators.PortfolioItems;
//using Common.Options;

//namespace Application.DependencyInjection
//{
//    public static class ApplicationDependencyInjection
//    {
//        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
//        {

//            services.AddOptions<ValidationOptions>()
//                    .BindConfiguration("Validation")
//                    .ValidateDataAnnotations();

//            services.AddAutoMapper(
//                    Assembly.GetExecutingAssembly(),
//                    typeof(ActivityLogProfile).Assembly,
//                    typeof(MessageProfile).Assembly,
//                    typeof(PortfolioItemProfile).Assembly
//);


//            // Application core
//            services.AddAutoMapper(Assembly.GetExecutingAssembly());
//            services.AddAutoMapper(typeof(ActivityLogProfile).Assembly);
//            services.AddAutoMapper(typeof(MessageProfile).Assembly);
//            services.AddMediatR(Assembly.GetExecutingAssembly());
//            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
//            services.AddValidatorsFromAssemblyContaining<CreatePortfolioItemDtoValidator>();
//            services.AddValidatorsFromAssemblyContaining<DeletePortfolioItemCommandValidator>();



//            // Common services
//            services.AddScoped<IUserContext, UserContext>();
//            services.AddScoped<IPasswordHasher, PasswordHasher>();
//            services.AddScoped<ILoggerService, LoggerService>();
//            services.AddScoped<IAuthorizationService, AuthorizationService>();
//            services.AddScoped<IActivityLoggerService, ActivityLoggerService>();
//            services.AddScoped<ICloudStorageService, GoogleCloudStorageService>();



//            //services.AddScoped<ITokenService, TokenService>();

//            // Validation pipeline
//            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
//            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));
//            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ExceptionHandlingBehavior<,>));
//            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
//            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehavior<,>));


//            // Infrastructure
//            services.AddHttpContextAccessor();

//            return services;

//        }
//    }
//}
