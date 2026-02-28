using CarMarketApp.Domain.Entities;

namespace CarMarketApp.Application.Abstractions.Repositories;

public interface IBrandRepository
{
    void Add(Brand brand);
    void Update(Brand brand);
    Task<Brand?> GetBrandByNameAsync(string normalizedName, CancellationToken cancellationToken);
    Task<Brand?> GetBrandByIdAsync(Guid brandId, CancellationToken cancellationToken);
    Task<Brand?> GetBrandByIdIgnoringQueryFilterAsync(Guid brandId, CancellationToken cancellationToken);
    IQueryable<Brand> GetAllBrandsQuery();
}
