namespace CarRentalApp.Models.Data
{
    public class User
    {
        public Guid Id { get; set; }

        public string Email { get; set; }
        public string Username { get; set; }        
        public string Password { get; set; }

        public DateOnly Birthday { get; set; }

        public Role Role { get; set; }
    }

    public enum Role
    {
        User,
        Admin
    }
}
