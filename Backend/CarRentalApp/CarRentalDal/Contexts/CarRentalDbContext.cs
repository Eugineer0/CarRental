using CarRentalDal.Models;
using Microsoft.EntityFrameworkCore;

namespace CarRentalDal.Contexts
{
    public class CarRentalDbContext : DbContext
    {
        public DbSet<User> Users => Set<User>();

        public DbSet<UserRole> UserRoles => Set<UserRole>();

        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

        public DbSet<Car> Cars => Set<Car>();

        public DbSet<CarType> CarTypes => Set<CarType>();

        public DbSet<Order> Orders => Set<Order>();

        public DbSet<OrderService> OrderServices => Set<OrderService>();

        public DbSet<Service> Services => Set<Service>();

        public DbSet<RentalCenter> RentalCenters => Set<RentalCenter>();

        public CarRentalDbContext(DbContextOptions<CarRentalDbContext> options) : base(options)
        {
        }
    }
}