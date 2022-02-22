namespace CarRentalApp.Models.DTOs;

public class AuthenticationDTO
{
    public string AccessToken { get; set; }
    
    public string RefreshToken { get; set; }
}