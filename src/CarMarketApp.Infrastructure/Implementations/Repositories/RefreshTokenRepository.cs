using CarMarketApp.Application.Abstractions.Repositories;
using CarMarketApp.Domain.Entities;
using CarMarketApp.Infrastructure.Persistence;

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
}
