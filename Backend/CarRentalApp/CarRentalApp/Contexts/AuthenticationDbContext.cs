using CarRentalApp.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarRentalApp.Models.Contexts
{
    public class AuthenticationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }        

        public AuthenticationDbContext(DbContextOptions<AuthenticationDbContext> options)
        : base(options)
        {
        }
    }
}
