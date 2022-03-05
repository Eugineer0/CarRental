using CarRentalApp.Models.Dal;

namespace CarRentalBll.Models
{
    public static class RolesInfo
    {
        public const string AdminRolesString = "Admin, SuperAdmin";
        public const string ClientRolesString = "Client";

        public static readonly IEnumerable<Roles> AdminRoles = new[] { Roles.Admin, Roles.SuperAdmin };
        public static readonly IEnumerable<Roles> ClientRoles = new[] { Roles.Client };

        public static bool Intersects(this IEnumerable<UserRole> roles, IEnumerable<Roles> candidate)
        {
            return roles.Select(role => role.Role).Intersect(candidate).Any();
        }

        public static bool Intersects(this IEnumerable<Roles> roles, IEnumerable<Roles> candidate)
        {
            return roles.Intersect(candidate).Any();
        }
    }
}