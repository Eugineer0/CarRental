namespace CarRentalApp.Models.BLL
{
    public class AccessModel
    {
        public string AccessToken { get; set; } = null!;

        public string RefreshToken { get; set; } = null!;
    }
}