using CarMarketApp.Domain.Entities;

namespace CarMarketApp.Application.Abstractions.Repositories;

public interface IRefreshTokenRepository
{
    void Add(RefreshToken refreshToken);
}
