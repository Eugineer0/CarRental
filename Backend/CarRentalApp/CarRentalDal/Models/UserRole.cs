using SharedResources;

namespace CarRentalDal.Models
{
    public class UserRole
    {
        public int Id { get; set; }

        public Roles Role { get; set; }

        public Guid UserId { get; set; }
    }
}