namespace CarRentalApp.Models.Entities;

public class RegistrationResponse
{
    public User? User { get; set; }

    public int StatusCode { get; set; }

    public string Response { get; set; }
}