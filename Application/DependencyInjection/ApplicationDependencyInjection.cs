using Application.Common.Behaviors;
using Application.Common.Interfaces;
using Application.Services;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using AutoMapper;

namespace Application.DependencyInjection
{
    public static class ApplicationDependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Application core
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            // Common services
            services.AddScoped<IUserContext, UserContext>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<ILoggerService, LoggerService>();
            //services.AddScoped<ITokenService, TokenService>();

            // Validation pipeline
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            // Infrastructure
            services.AddHttpContextAccessor();

            return services;
        }
    }
}
