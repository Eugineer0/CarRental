namespace CarRentalApp.Models.DTOs.Responces
{
    public class AuthenticationResponse
    {
        public string AccesToken { get; set; }
        public DateTime ExpiresAt { get; set; }

        public AuthenticationResponse(string tokenString, DateTime validTo)
        {
            AccesToken = tokenString;
            ExpiresAt = validTo;
        }
    }
}