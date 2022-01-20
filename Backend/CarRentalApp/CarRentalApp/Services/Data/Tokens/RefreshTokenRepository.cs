using Microsoft.EntityFrameworkCore;
using CarRentalApp.Contexts;
using CarRentalApp.Models.Entities;

namespace CarRentalApp.Services.Data
{
    public class RefreshTokenRepository
    {
        private readonly CarRentalDbContext _authenticationDbContext;

        public RefreshTokenRepository(CarRentalDbContext authenticationDbContext)
        {
            _authenticationDbContext = authenticationDbContext;
        }

        public async Task<RefreshToken> CreateAsync(RefreshToken refreshToken)
        {
            _authenticationDbContext.RefreshTokens.Add(refreshToken);
            await _authenticationDbContext.SaveChangesAsync();

            return refreshToken;
        }

        public async Task DeleteAsync(RefreshToken refreshToken)
        {
            _authenticationDbContext.RefreshTokens.Remove(refreshToken);
            await _authenticationDbContext.SaveChangesAsync();
        }

        public async Task DeleteRelatedTokensAsync(Guid userId)
        {
            var tokens = await _authenticationDbContext
                .RefreshTokens
                .Where(t => t.UserId == userId)
                .ToListAsync();

            _authenticationDbContext.RefreshTokens.RemoveRange(tokens);
            await _authenticationDbContext.SaveChangesAsync();
        }

        public async Task<RefreshToken?> GetByTokenAsync(string refreshTokenString)
        {
            return await _authenticationDbContext.RefreshTokens.FirstOrDefaultAsync(t => t.Token == refreshTokenString);
        }        
    }
}