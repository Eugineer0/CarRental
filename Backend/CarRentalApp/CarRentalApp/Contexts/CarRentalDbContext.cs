using CarRentalApp.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarRentalApp.Contexts
{
    public class CarRentalDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        
        public DbSet<UserRole> UserRoles { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }
        
        public DbSet<Car> Cars { get; set; }
        
        public DbSet<CarType> CarTypes { get; set; }
        
        public DbSet<Order> Orders { get; set; }
        
        public DbSet<OrderService> OrderServices { get; set; }
        
        public DbSet<RentalCenter> RentalCenters { get; set; }

        public CarRentalDbContext(DbContextOptions<CarRentalDbContext> options) : base(options)
        {
        }
    }
}