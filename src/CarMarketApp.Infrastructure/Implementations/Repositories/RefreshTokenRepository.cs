using CarMarketApp.Application.Abstractions.Repositories;
using CarMarketApp.Domain.Entities;
using CarMarketApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CarMarketApp.Infrastructure.Implementations.Repositories;

public sealed class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly ApplicationDbContext _context;

    public RefreshTokenRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public void Add(RefreshToken refreshToken)
    {
        _context.Add(refreshToken);
    }

    public void Update(RefreshToken refreshToken)
    {
        _context.Update(refreshToken);
    }

    public async Task<RefreshToken?> GetRefreshTokenByToken(string tokenHash, CancellationToken cancellationToken)
    {
        return await _context.RefreshTokens.FirstOrDefaultAsync(rt => string.Equals(rt.TokenHash, tokenHash));
    }

    public async Task<IEnumerable<RefreshToken>> GetRefreshTokensByUserId(Guid userId, CancellationToken cancellationToken)
    {
        return await _context.RefreshTokens.Where(rt => rt.AppUserId == userId).ToListAsync(cancellationToken);
    }
}
