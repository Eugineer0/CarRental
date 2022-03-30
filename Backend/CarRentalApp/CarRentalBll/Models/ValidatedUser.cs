namespace CarRentalBll.Models
{
    public class ValidatedUser
    {
        public UserModel User { get; set; } = null!;

        public UserStatus Status { get; set; }
    }

    public enum UserStatus
    {
        Ok,
        NotEnoughInfo,
        Unapproved
    }
}