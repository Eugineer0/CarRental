namespace CarRentalApp.Models.Entities
{
    public class RefreshToken
    {
        public Guid Id { get; set; }

        public string Token { get; set; } = null!;

        public Guid UserId { get; set; }
    }
}