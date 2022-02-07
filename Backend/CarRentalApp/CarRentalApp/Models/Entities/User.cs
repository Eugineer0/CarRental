namespace CarRentalApp.Models.Entities
{
    public class User
    {
        public Guid Id { get; set; }

        public string Email { get; set; }

        public string Username { get; set; }

        public byte[] HashedPassword { get; set; }

        public byte[] Salt { get; set; }

        public DateTime Birthday { get; set; }

        public Role Role { get; set; }
    }

    public enum Role
    {
        None,
        Client,
        Admin,
        SuperAdmin
    }
}