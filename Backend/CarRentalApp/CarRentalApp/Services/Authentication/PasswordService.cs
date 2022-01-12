namespace CarRentalApp.Services.Authentication
{
    public abstract class PasswordService
    {
        public abstract string GenerateSalt();

        public abstract string DigestPassword(string password, string salt);

        public bool VerifyPassword(string validPassword, string salt, string password)
        {
            var hashedPassword = DigestPassword(password, salt);
            return validPassword.Equals(hashedPassword);
        }
    }
}