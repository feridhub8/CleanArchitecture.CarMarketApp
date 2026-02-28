using CarMarketApp.Application.DTOs.Brands;
using CarMarketApp.Application.Models;
using CarMarketApp.Application.Models.ResultPattern;
using CarMarketApp.Domain.Entities;

namespace CarMarketApp.Application.Abstractions.Services;

public interface IBrandService
{
    Task<Result> CreateBrandAsync(CreateBrandDto createBrandDto, CancellationToken cancellationToken);
    Task<Result> UpdateBrandAsync(UpdateBrandDto updateBrandDto, CancellationToken cancellationToken);
    Task<Result> DeleteBrandAsync(Guid brandId, CancellationToken cancellationToken);
    Task<Result> RestoreBrandAsync(Guid brandId, CancellationToken cancellationToken);
    Task<Result<PagedList<BrandDto>>> GetAllBrandsAsync(BrandFilterDto brandFilterDto, CancellationToken cancellationToken);
}
