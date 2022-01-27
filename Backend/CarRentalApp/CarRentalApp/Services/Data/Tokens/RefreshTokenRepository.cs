using System;
using System.Linq;
using System.Threading.Tasks;
using CarRentalApp.Contexts;
using CarRentalApp.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarRentalApp.Services.Data.Tokens
{
    public class RefreshTokenRepository
    {
        private readonly CarRentalDbContext _carRentalDbContext;

        public RefreshTokenRepository(CarRentalDbContext carRentalDbContext)
        {
            _carRentalDbContext = carRentalDbContext;
        }

        public async Task<RefreshToken> CreateAsync(RefreshToken refreshToken)
        {
            _carRentalDbContext.RefreshTokens.Add(refreshToken);
            await _carRentalDbContext.SaveChangesAsync();

            return refreshToken;
        }

        public async Task DeleteAsync(RefreshToken refreshToken)
        {
            _carRentalDbContext.RefreshTokens.Remove(refreshToken);
            await _carRentalDbContext.SaveChangesAsync();
        }

        public async Task DeleteRelatedTokensAsync(Guid userId)
        {
            var tokens = await _carRentalDbContext
                .RefreshTokens
                .Where(t => t.UserId == userId)
                .ToListAsync();

            _carRentalDbContext.RefreshTokens.RemoveRange(tokens);
            await _carRentalDbContext.SaveChangesAsync();
        }

        public async Task<RefreshToken?> GetByTokenAsync(string refreshTokenString)
        {
            return await _carRentalDbContext.RefreshTokens.FirstOrDefaultAsync(t => t.Token == refreshTokenString);
        }        
    }
}