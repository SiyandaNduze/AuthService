using Auth.Application.Interfaces;
using Auth.Domain;
using Auth.Domain.Entities;
using Auth.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Auth.Infrastructure
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly AuthDbContext _db;

        public RefreshTokenRepository(AuthDbContext db)
        {
            _db = db;
        }

        public Task<RefreshToken?> GetAsync(string token)
            => _db.RefreshTokens.FirstOrDefaultAsync(x => x.Token == token);

        public async Task AddAsync(RefreshToken token)
        {
            _db.RefreshTokens.Add(token);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(RefreshToken token)
        {
            _db.RefreshTokens.Update(token);
            await _db.SaveChangesAsync();
        }
    }
}