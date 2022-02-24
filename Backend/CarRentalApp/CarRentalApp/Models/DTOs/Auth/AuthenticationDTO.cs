namespace CarRentalApp.Models.DTOs.Auth;

public class AuthenticationDTO
{
    public string AccessToken { get; set; }

    public string RefreshToken { get; set; }
}