using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Common.GlobalResponse;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Authentication;

public static class JwtAuthenticationExtension
{
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var key = configuration["JWT:Secret"];
        var issuer = configuration["JWT:ValidIssuer"];
        var audience = configuration["JWT:ValidAudience"];

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key!))
                };

                options.Events = new JwtBearerEvents
                {
                    OnChallenge = context =>
                    {
                        context.HandleResponse();
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        context.Response.ContentType = "application/json";
                        var result = JsonSerializer.Serialize(new ResponseModel
                        {
                            IsSuccess = false,
                            Errors = new List<string> { "Authentication required. Please provide a valid token." }
                        });
                        return context.Response.WriteAsync(result);
                    },
                    OnForbidden = context =>
                    {
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        context.Response.ContentType = "application/json";
                        var result = JsonSerializer.Serialize(new ResponseModel
                        {
                            IsSuccess = false,
                            Errors = new List<string> { "Access denied. You do not have permission to perform this action." }
                        });
                        return context.Response.WriteAsync(result);
                    }
                };
            });

        return services;
    }
}
