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
        IRefreshTokenRepository refreshTokenRepository)
    {
        _context = context;
        RefreshTokens = refreshTokenRepository;
    }
    public IRefreshTokenRepository RefreshTokens { get; }

    public async Task BeginTransactionAsync()
    {
        if (_transaction != null)
            return;

        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _context.SaveChangesAsync(cancellationToken);
            await _transaction!.CommitAsync();
        }
        catch
        {
            await RollbackTransactionAsync();
            throw;
        }
        finally
        {
            await _transaction!.DisposeAsync();
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
