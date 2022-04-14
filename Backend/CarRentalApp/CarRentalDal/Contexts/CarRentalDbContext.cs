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

        public DbSet<CarTypeCarService> CarTypeCarServices => Set<CarTypeCarService>();

        public DbSet<CarTypeCarServicePrices> CarTypeCarServicePrices => Set<CarTypeCarServicePrices>();

        public DbSet<Order> Orders => Set<Order>();

        public DbSet<CarService> CarServices => Set<CarService>();

        public DbSet<OrderCarService> OrderCarServices => Set<OrderCarService>();

        public DbSet<RentalCenter> RentalCenters => Set<RentalCenter>();

        public CarRentalDbContext(DbContextOptions<CarRentalDbContext> options) : base(options)
        {
        }
    }
}