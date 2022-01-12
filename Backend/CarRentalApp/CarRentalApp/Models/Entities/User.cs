namespace CarRentalApp.Models.Entities
{
    public class User
    {
        public Guid Id { get; set; }

        public string Email { get; set; }
        public string Username { get; set; }
        public string HashedPassword { get; set; }
        public string Salt { get; set; }

        public DateTime Birthday { get; set; }

        public Role Role { get; set; }
    }

    public enum Role
    {
        User,
        Admin
    }
}