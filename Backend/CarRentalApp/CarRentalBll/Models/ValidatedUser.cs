namespace CarRentalBll.Models
{
    public class ValidatedUser
    {
        public UserModel User { get; set; } = null!;

        public UserStatuses Status { get; set; }
    }

    public enum UserStatuses
    {
        Ok,
        NotEnoughInfo,
        Unapproved
    }
}