using Application.Common.Behaviors;
using Application.Common.Interfaces;
using Application.CQRS.ClientProfiles.Queries.Handlers;
using Application.Mappings;
using Application.Services;
using Application.Validators.ServiceBookings;
using Common.Options;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application.DependencyInjection
{
    public static class ApplicationDependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Validators
            services.AddValidatorsFromAssembly(typeof(CreateServiceBookingCommandValidator).Assembly);
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            // Options 
            services.AddOptions<ValidationOptions>()
                .BindConfiguration("Validation")
                .ValidateDataAnnotations();

            // AutoMapper Profiles 
            services.AddAutoMapper(
                Assembly.GetExecutingAssembly(),
                typeof(ActivityLogProfile).Assembly,
                typeof(MessageProfile).Assembly,
                typeof(PortfolioItemProfile).Assembly,
                typeof(ServiceBookingProfile).Assembly
            );

            // MediatR
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddMediatR(typeof(GetClientProfileByUserIdQueryHandler).Assembly);

            // Pipelines
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LocalizedValidationBehavior<,>));

            return services;
        }
    }
}

