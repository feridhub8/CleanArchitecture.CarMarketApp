using CarMarketApp.Application.Abstractions.Repositories;
using CarMarketApp.Domain.Entities;
using CarMarketApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CarMarketApp.Infrastructure.Implementations.Repositories;

public sealed class BrandRepository : IBrandRepository
{
    private readonly ApplicationDbContext _context;

    public BrandRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public void Add(Brand brand)
    {
        _context.Add(brand);
    }

    public void Update(Brand brand)
    {
        _context.Update(brand);
    }

    public async Task<Brand?> GetBrandByNameAsync(string normalizedName, CancellationToken cancellationToken)
    {
        return await _context.Brands.FirstOrDefaultAsync(b => b.NormalizedName == normalizedName, cancellationToken);
    }

    public async Task<Brand?> GetBrandByIdAsync(Guid brandId, CancellationToken cancellationToken)
    {
        return await _context.Brands.FirstOrDefaultAsync(b => b.Id == brandId, cancellationToken);
    }

    public async Task<Brand?> GetBrandByIdIgnoringQueryFilterAsync(Guid brandId, CancellationToken cancellationToken)
    {
        return await _context.Brands.IgnoreQueryFilters().FirstOrDefaultAsync(b => b.Id == brandId, cancellationToken);
    }

    public IQueryable<Brand> GetAllBrandsQuery()
    {
        return _context.Brands.AsQueryable();
    }
}
