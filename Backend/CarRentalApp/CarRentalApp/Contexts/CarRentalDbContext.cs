using CarRentalApp.Models.DAL;
using Microsoft.EntityFrameworkCore;

namespace CarRentalApp.Contexts
{
    public class CarRentalDbContext : DbContext
    {
        public DbSet<User> Users => Set<User>();

        public DbSet<UserRole> UserRoles => Set<UserRole>();

        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

        public CarRentalDbContext(DbContextOptions<CarRentalDbContext> options) : base(options)
        {
        }
    }
}