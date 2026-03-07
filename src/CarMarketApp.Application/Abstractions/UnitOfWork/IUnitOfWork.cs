using CarMarketApp.Application.Abstractions.Repositories;

namespace CarMarketApp.Application.Abstractions.UnitOfWork;

public interface IUnitOfWork : IDisposable
{
    IRefreshTokenRepository RefreshTokens { get; }
    IBrandRepository Brands { get; }
    IModelRepository Models { get; }
    IAdvertRepository Adverts { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    Task BeginTransactionAsync();
    Task<int> CommitTransactionAsync(CancellationToken cancellationToken);
    Task RollbackTransactionAsync();
}
