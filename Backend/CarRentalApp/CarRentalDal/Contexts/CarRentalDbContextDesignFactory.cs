using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CarRentalDal.Contexts;

public class CarRentalDbContextDesignFactory : IDesignTimeDbContextFactory<CarRentalDbContext>
{
    public CarRentalDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<CarRentalDbContext>();
        optionsBuilder.UseSqlServer("Server=WSB-084-74\\MSSQSERVER1;Database=CarRentalDB;Trusted_Connection=True;");

        return new CarRentalDbContext(optionsBuilder.Options);
    }
}