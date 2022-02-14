using System.ComponentModel.DataAnnotations;

namespace CarRentalApp.Models.Entities
{
    public class UserRole
    {
        public const string AdminRolesString = "Admin, SuperAdmin";
        public const string ClientRolesString = "Client";

        public static readonly IEnumerable<Roles> AdminRoles = AdminRolesString
            .Split(',')
            .Select(role => (Roles) Enum.Parse(typeof(Roles), role));

        public static readonly IEnumerable<Roles> ClientRoles = ClientRolesString
            .Split(',')
            .Select(role => (Roles) Enum.Parse(typeof(Roles), role));

        [Key] public int EntryId { get; set; }

        public Roles Role { get; set; }

        public Guid UserId { get; set; }
    }

    public enum Roles : byte
    {
        None = 0,
        Client = 1,
        Admin = 10,
        SuperAdmin = 20
    }
}