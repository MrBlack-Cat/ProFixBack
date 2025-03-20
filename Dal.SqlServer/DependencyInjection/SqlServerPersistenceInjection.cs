using DAL.SqlServer.Context;
using DAL.SqlServer.UnitOfWork;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Repository.Common;
using System.Data;

namespace DAL.SqlServer.DependencyInjection
{
    public static class SqlServerPersistenceInjection
    {
        public static IServiceCollection AddSqlServerPersistence(this IServiceCollection services, string connectionString)
        {
            // Add EF DbContext
            //services.AddDbContext<AppDbContext>(options =>
            //    options.UseSqlServer(connectionString));

            // Add IDbConnection for Dapper
            services.AddScoped<IDbConnection>(sp =>
            {
                var connection = new SqlConnection(connectionString);
                connection.Open();
                return connection;
            });

            // Register UnitOfWork
            services.AddScoped<IUnitOfWork, SqlUnitOfWork>();

            return services;
        }
    }
}
