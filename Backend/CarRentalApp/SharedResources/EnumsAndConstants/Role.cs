namespace SharedResources.EnumsAndConstants
{
    public enum Role : byte
    {
        None,
        Client,
        Admin,
        SuperAdmin
    }

    public class RolesConstants
    {
        public const string AdminRolesString = "Admin, SuperAdmin";
        public const string ClientRolesString = "Client";

        public static readonly IEnumerable<Role> AdminRoles = new[] { Role.Admin, Role.SuperAdmin };
        public static readonly IEnumerable<Role> ClientRoles = new[] { Role.Client };
    }
}