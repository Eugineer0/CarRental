using System.Security.Cryptography;
using System.Text;

namespace CarRentalApp.Services.Authentication
{
    public class ShaPasswordService : PasswordService
    {
        public override string DigestPassword(string password, string salt)
        {
            var hashFunc = SHA256.Create();

            var data = Encoding.UTF8.GetBytes(password + salt);

            var hashedPassword = hashFunc.ComputeHash(data);

            return Convert.ToBase64String(hashedPassword);
        }

        public override string GenerateSalt()
        {
            var random = new Random();            
            
            var salt = new byte[32];
            
            random.NextBytes(salt);
            
            return Convert.ToBase64String(salt);
        }        
    }
}