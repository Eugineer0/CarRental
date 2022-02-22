namespace CarRentalApp.Models.Entities;

public class RentalCenter
{
    public Guid Id { get; set; }

    public string Name { get; set; }
    
    public string Country { get; set; }
    
    public string City { get; set; }
    
    public string Address { get; set; }

    public ICollection<Car> Cars { get; set; }
}