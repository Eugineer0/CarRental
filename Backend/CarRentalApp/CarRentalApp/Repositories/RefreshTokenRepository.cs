using CarRentalApp.Contexts;
using CarRentalApp.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarRentalApp.Repositories
{
    public class RefreshTokenRepository
    {
        private readonly CarRentalDbContext _carRentalDbContext;

        public RefreshTokenRepository(CarRentalDbContext carRentalDbContext)
        {
            _carRentalDbContext = carRentalDbContext;
        }

        public Task InsertAsync(RefreshToken refreshToken)
        {
            _carRentalDbContext.RefreshTokens.Add(refreshToken);
            return _carRentalDbContext.SaveChangesAsync();
        }

        public Task DeleteAsync(RefreshToken refreshToken)
        {
            _carRentalDbContext.RefreshTokens.Remove(refreshToken);
            return _carRentalDbContext.SaveChangesAsync();
        }

        public async Task<bool> DeleteRelatedTokensByUserIdAsync(Guid userId)
        {
            var tokens = await _carRentalDbContext.RefreshTokens
                .Where(t => t.UserId == userId)
                .ToListAsync();

            _carRentalDbContext.RefreshTokens.RemoveRange(tokens);
            
            try
            {
                await _carRentalDbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }

        public async Task<RefreshToken?> GetByTokenStringAsync(string refreshTokenString)
        {
            return await _carRentalDbContext.RefreshTokens.FirstOrDefaultAsync(t => t.Token == refreshTokenString);
        }
    }
}