namespace CarRentalApp.Models.BLL;

public class RentalCenterModel
{
    public string Name { get; set; }

    public string Country { get; set; }

    public string City { get; set; }

    public string Address { get; set; }

    public IEnumerable<CarModel> Cars { get; set; }
}