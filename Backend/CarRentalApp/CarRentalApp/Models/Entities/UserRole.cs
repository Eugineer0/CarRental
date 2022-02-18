namespace CarRentalApp.Models.Entities
{
    public class UserRole
    {
        public const string AdminRolesString = "Admin, SuperAdmin";
        public const string ClientRolesString = "Client";

        public static readonly IEnumerable<Roles> AdminRoles = new[] {Roles.Admin, Roles.SuperAdmin};
        public static readonly IEnumerable<Roles> ClientRoles = new[] {Roles.Client};

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