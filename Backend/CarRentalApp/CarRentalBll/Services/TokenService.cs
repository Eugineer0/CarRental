using CarRentalDal.Contexts;
using CarRentalDal.Models;
using Microsoft.EntityFrameworkCore;

namespace CarRentalBll.Services
{
    public class TokenService
    {
        private readonly CarRentalDbContext _carRentalDbContext;

        public TokenService(CarRentalDbContext carRentalDbContext)
        {
            _carRentalDbContext = carRentalDbContext;
        }

        /// <summary>
        /// Removes token model found by <paramref name="refreshToken"/> and returns it.
        /// </summary>
        /// <param name="refreshToken">ejected token string.</param>
        /// <returns>token model, removed from database.</returns>
        public async Task<RefreshToken?> PopTokenAsync(string refreshToken)
        {
            var token = await _carRentalDbContext.RefreshTokens
                .FirstOrDefaultAsync(t => t.Token == refreshToken);
            if (token == null)
            {
                return null;
            }

            _carRentalDbContext.RefreshTokens.Remove(token);
            await _carRentalDbContext.SaveChangesAsync();

            return token;
        }

        /// <summary>
        /// Saves refresh token model based on <paramref name="token"/> and <paramref name="userId"/>.
        /// </summary>
        /// <param name="token">token prototype to be saved.</param>
        /// <param name="userId">token model field.</param>
        public Task StoreTokenAsync(string token, Guid userId)
        {
            var refreshToken = new RefreshToken()
            {
                Token = token,
                UserId = userId
            };

            _carRentalDbContext.RefreshTokens.Add(refreshToken);

            return _carRentalDbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Revokes all stored tokens with specified <paramref name="userId"/>.
        /// </summary>
        /// <param name="userId">token model field.</param>
        public async Task RevokeTokensByAsync(Guid userId)
        {
            var tokens = await _carRentalDbContext.RefreshTokens
                .Where(t => t.UserId == userId)
                .ToListAsync();

            _carRentalDbContext.RefreshTokens.RemoveRange(tokens);

            await _carRentalDbContext.SaveChangesAsync();
        }
    }
}