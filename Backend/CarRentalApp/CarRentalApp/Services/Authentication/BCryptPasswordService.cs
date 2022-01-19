namespace CarRentalApp.Services.Authentication
{
    public class BCryptPasswordService : PasswordService
    {
        public override string GenerateSalt()
        {
            return BCrypt.Net.BCrypt.GenerateSalt();
        }

        public override string DigestPassword(string password, string salt)
        {
            return BCrypt.Net.BCrypt.HashPassword(password + salt);
        }        
    }
}