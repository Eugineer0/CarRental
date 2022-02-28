namespace CarRentalApp.Models.DTOs.RentalCenter;

public class CenterFilterRequest
{
    public string? Country { get; set; }

    public string? City { get; set; }

    public int MinimumAvailableCarsNumber { get; set; }

    public DateTime? StartRent { get; set; }

    public DateTime? FinishRent { get; set; }
}