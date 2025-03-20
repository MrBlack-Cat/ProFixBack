using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Common.DependencyInjection
{
    public static class SwaggerDependencyInjection
    {
        public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
            });

            return services;
        }
    }
}
