using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace CarRentalApp.Services.Authentication
{
    public class PasswordService
    {
        public byte[] DigestPassword(string password, byte[] salt)
        {           
            var data = (byte[])Encoding.UTF8.GetBytes(password).Concat(salt);

            var hashFunc = SHA256.Create();   
            
            return hashFunc.ComputeHash(data);
        }

        public byte[] GenerateSalt()
        {
            var random = new Random();            
            
            var salt = new byte[31];
            
            random.NextBytes(salt);
            
            return salt;
        }

        public bool VerifyPassword(byte[] validPassword, byte[] salt, string password)
        {
            var hashedPassword = DigestPassword(password, salt);
            return validPassword.SequenceEqual(hashedPassword);
        }
    }
}