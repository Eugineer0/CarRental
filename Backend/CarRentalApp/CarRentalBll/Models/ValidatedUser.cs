namespace CarRentalBll.Models
{
    public class ValidatedUser
    {
        public UserModel User { get; set; } = null!;

        public ValidationStates ValidationState { get; set; }
    }

    public enum ValidationStates
    {
        Ok,
        InvalidPassword,
        NotEnoughInfo,
        Unapproved
    }
}