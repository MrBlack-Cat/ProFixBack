using Domain.Entities;
using Domain.Entities.TokenSecurity;
using Microsoft.EntityFrameworkCore;



namespace DAL.SqlServer.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Post>Posts { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<ServiceBooking> ServiceBookings { get; set; }
        public DbSet<ServiceProviderProfile> ServiceProviderProfiles { get; set; }
        public DbSet<SubscriptionPlan> SubscriptionPlans { get; set; }
        public DbSet<SupportTicket> SupportTickets { get; set; }

        //sonradan elave
        public DbSet <RefreshToken> RefreshTokens { get; set; }

      

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Fluent API
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}
