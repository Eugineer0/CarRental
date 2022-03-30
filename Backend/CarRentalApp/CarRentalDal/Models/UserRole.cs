using SharedResources.EnumsAndConstants;

namespace CarRentalDal.Models
{
    public class UserRole
    {
        public int Id { get; set; }

        public Role Role { get; set; }

        public Guid UserId { get; set; }
    }
}