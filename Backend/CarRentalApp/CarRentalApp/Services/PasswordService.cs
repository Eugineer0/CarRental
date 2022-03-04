﻿using System.Security.Cryptography;
using System.Text;
using CarRentalApp.Exceptions;

namespace CarRentalApp.Services
{
    public class PasswordService
    {
        /// <summary>
        /// Concatenates <paramref name="password"/> with <paramref name="salt"/> and returns its hash.
        /// </summary>
        /// <param name="password">string representation to user`s secret key.</param>
        /// <param name="salt">byte array used in password hashing.</param>
        /// <returns>Hashed <paramref name="password"/>.</returns>
        public byte[] DigestPassword(string password, byte[] salt)
        {
            var data = Encoding.UTF8.GetBytes(password).Concat(salt).ToArray();

            using var hashFunc = SHA256.Create();
            return hashFunc.ComputeHash(data);
        }

        /// <summary>
        /// Generates random byte array of constant size.
        /// </summary>
        /// <returns>Generated byte array.</returns>
        public byte[] GenerateSalt()
        {
            var random = new Random();
            var salt = new byte[32];
            random.NextBytes(salt);

            return salt;
        }

        /// <summary>
        /// Hashes <paramref name="password"/> and compares it with <paramref name="validPassword"/>.
        /// </summary>
        /// <param name="validPassword">valid hashed password.</param>
        /// <param name="salt">byte array used in password hashing.</param>
        /// <param name="password">string representation to validate.</param>
        /// <exception cref="SharedException">Incorrect username or password.</exception>
        public void ValidatePassword(byte[] validPassword, byte[] salt, string password)
        {
            var hashedPassword = DigestPassword(password, salt);
            if (!validPassword.SequenceEqual(hashedPassword))
            {
                throw new SharedException(
                    ErrorTypes.AuthFailed,
                    "Incorrect username or password",
                    "Incorrect password"
                );
            }
        }
    }
}