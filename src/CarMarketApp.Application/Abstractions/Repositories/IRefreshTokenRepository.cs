using CarMarketApp.Domain.Entities;

namespace CarMarketApp.Application.Abstractions.Repositories;

public interface IRefreshTokenRepository
{
    void Add(RefreshToken refreshToken);
    void Update(RefreshToken refreshToken);
    Task<RefreshToken?> GetRefreshTokenByToken(string tokenHash, CancellationToken cancellationToken);
    Task<IEnumerable<RefreshToken>> GetRefreshTokensByUserId(Guid userId, CancellationToken cancellationToken);
}
