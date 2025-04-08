using Api.Middleware;
using Application.Common.Interfaces;
using Application.Common.Services;
using Application.DependencyInjection;
using Application.Mappings;
using Application.Services;
using DAL.SqlServer.Context;
using DAL.SqlServer.DependencyInjection;
using DAL.SqlServer.UnitOfWork;
using Infrastructure.Authentication;
using Infrastructure.Behaviors;
using Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Repository.Common;
using System.Data;
using System.Text;


namespace WebApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);


        #region JWTtoken


        var jwtSettings = builder.Configuration.GetSection("JWT");
        var secretKey = jwtSettings.GetValue<string>("Secret");



        builder.Services.AddSwaggerGen(c =>
        {
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "Enter JWT token (Bearer {token})",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new List<string>()
        }
    });
        });

        #region CORS
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("ProFixCors", policy =>
            {
                policy.WithOrigins(
                    "http://localhost:5173", "http://localhost:5174", "http://localhost:5175",    // Front
                    "https://admin.profix.com"     // Test Admin
                )
                .AllowAnyHeader()
                .AllowAnyMethod();
            });
        });
        #endregion

        #region Database

        var conn = builder.Configuration.GetConnectionString("myconn");
        builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(conn));

        #endregion

        builder.Services.AddScoped<IDbConnection>(sp =>
        {
            var config = sp.GetRequiredService<IConfiguration>();
            var connection = new SqlConnection(config.GetConnectionString("myconn"));
            connection.Open(); // Arasdir. Qalsin ya yox
            return connection;
        });



        #endregion
        builder.Services.AddJwtAuthentication(builder.Configuration);
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddAutoMapper(typeof(PostProfile).Assembly);
        builder.Services.AddMediatR(typeof(Application.CQRS.Users.Handlers.RegisterUserHandler.Handler).Assembly);
        builder.Services.AddTransient<ITokenService, TokenService>();
        builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddScoped<IUserContext, UserContext>();
        builder.Services.AddApplicationServices();
        builder.Services.AddScoped<IUnitOfWork, SqlUnitOfWork>();
        builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ExceptionHandlingBehavior<,>));
        builder.Services.AddScoped<IActivityLoggerService, Application.Common.Services.ActivityLoggerService>();





        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "ProFix API V1");
                options.RoutePrefix = "swagger";

            });
        }






        app.UseHttpsRedirection();
        app.UseCors("ProFixCors");
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseMiddleware<ExceptionHandlingMiddleware>();

 

        //app.UseCors("AllowCors");

        app.MapControllers();

        app.Run();



    }
}