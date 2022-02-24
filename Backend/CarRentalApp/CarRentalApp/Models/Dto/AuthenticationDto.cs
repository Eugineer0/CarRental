namespace CarRentalApp.Models.Dto
{
    public class AuthenticationDto
    {
        public string AccessToken { get; set; } = null!;

        public string RefreshToken { get; set; } = null!;
    }
}