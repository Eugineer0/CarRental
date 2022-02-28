namespace CarRentalApp.Models.DTOs.RentalCenter;

public class RentalCenterDTO
{
    public string Name { get; set; }

    public string Country { get; set; }

    public string City { get; set; }

    public string Address { get; set; }

    public int? AvailableCarsNumber { get; set; }
}