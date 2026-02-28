using CarMarketApp.Application.Abstractions.Repositories;
using CarMarketApp.Application.Abstractions.UnitOfWork;
using CarMarketApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore.Storage;

namespace CarMarketApp.Infrastructure.Implementations.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private IDbContextTransaction? _transaction;
    public UnitOfWork(
        ApplicationDbContext context,
        IRefreshTokenRepository refreshTokenRepository,
        IBrandRepository brandRepository)
    {
        _context = context;
        RefreshTokens = refreshTokenRepository;
        Brands = brandRepository;
    }
    public IRefreshTokenRepository RefreshTokens { get; }
    public IBrandRepository Brands { get; }

    public async Task BeginTransactionAsync()
    {
        if (_transaction is not null)
            return;

        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task<int> CommitTransactionAsync(CancellationToken cancellationToken)
    {
        if (_transaction is null)
            throw new InvalidOperationException("Transaction has not been started.");

        try
        {
            int changeCount = await _context.SaveChangesAsync(cancellationToken);

            await _transaction.CommitAsync(cancellationToken);

            return changeCount;
        }
        catch
        {
            await _transaction.RollbackAsync(cancellationToken);
            throw;
        }
        finally
        {
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task RollbackTransactionAsync()
    {
        if (_transaction == null) return;

        await _transaction.RollbackAsync();
        await _transaction.DisposeAsync();
        _transaction = null;
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        => _context.SaveChangesAsync(cancellationToken);

    public void Dispose()
    {
        _transaction?.Dispose();
    }
}
