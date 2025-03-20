using Dal.SqlServer.Infrastructure;
using DAL.SqlServer.Context;
using DAL.SqlServer.Infrastructure;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Repository.Common;
using Repository.Repositories;
using System.Data;

namespace DAL.SqlServer.UnitOfWork
{
    public class SqlUnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private readonly IDbConnection _dbConnection;

        public SqlUnitOfWork(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _dbConnection = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
            _dbConnection.Open(); 
        }

        // Repositories
        private IUserRepository _userRepository;
        private IClientProfileRepository _clientProfileRepository;
        private IServiceProviderProfileRepository _serviceProviderProfileRepository;
        private ICertificateRepository _certificateRepository;
        private IPostRepository _postRepository;
        private IReviewRepository _reviewRepository;

        public IUserRepository UserRepository => _userRepository ??= new SqlUserRepository(_dbConnection);

        public IClientProfileRepository ClientProfileRepository => _clientProfileRepository ??= new SqlClientProfileRepository(_dbConnection);

        public IServiceProviderProfileRepository ServiceProviderProfileRepository =>_serviceProviderProfileRepository ??= new SqlServiceProviderProfileRepository(_dbConnection);

        public ICertificateRepository CertificateRepository =>_certificateRepository ??= new SqlCertificateRepository(_dbConnection);

        public IPostRepository PostRepository =>_postRepository ??= new SqlPostRepository(_dbConnection);

        public IReviewRepository ReviewRepository =>_reviewRepository ??= new SqlReviewRepository(_dbConnection);

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
