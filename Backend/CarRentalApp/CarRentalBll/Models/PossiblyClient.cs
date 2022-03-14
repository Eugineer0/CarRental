namespace CarRentalBll.Models
{
    public class PossiblyClient
    {
        public UserModel User { get; set; } = null!;

        public bool IsClient { get; set; }
    }
}