using CarRentalApp.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarRentalApp.Contexts
{
    public class CarRentalDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        
        public DbSet<Roles> UserRoles { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public CarRentalDbContext(DbContextOptions<CarRentalDbContext> options) : base(options)
        {
        }
    }
}