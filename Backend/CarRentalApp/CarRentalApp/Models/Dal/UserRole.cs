namespace CarRentalApp.Models.Dal
{
    public class UserRole
    {
        public int Id { get; set; }

        public Roles Role { get; set; }

        public Guid UserId { get; set; }
    }

    public enum Roles : byte
    {
        None,
        Client,
        Admin,
        SuperAdmin
    }
}