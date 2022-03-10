using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace CarRentalDal.Contexts
{
    public class CarRentalDbContextDesignFactory : IDesignTimeDbContextFactory<CarRentalDbContext>
    {
        public CarRentalDbContext CreateDbContext(string[] args)
        {
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../CarRentalWeb"))
                .AddJsonFile("appsettings.json")
                .Build();

            var configString = config.GetConnectionString("CarRentalDB");

            var optionsBuilder = new DbContextOptionsBuilder<CarRentalDbContext>();
            optionsBuilder.UseSqlServer(configString);

            return new CarRentalDbContext(optionsBuilder.Options);
        }
    }
}