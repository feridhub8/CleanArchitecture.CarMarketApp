using CarMarketApp.Application.Abstractions.Services;
using CarMarketApp.Application.DTOs.Brands;
using CarMarketApp.Application.Models;
using CarMarketApp.Application.Models.ResultPattern;
using CarMarketApp.Domain.Entities;
using MediatR;

namespace CarMarketApp.Application.Features.Queries.Brands;

public sealed class GetAllBrandsQueryHandler : IRequestHandler<GetAllBrandsQuery, Result<PagedList<BrandDto>>>
{
    private readonly IBrandService _brandService;

    public GetAllBrandsQueryHandler(IBrandService brandService)
    {
        _brandService = brandService;
    }

    public async Task<Result<PagedList<BrandDto>>> Handle(GetAllBrandsQuery request, CancellationToken cancellationToken)
    {
        return await _brandService.GetAllBrandsAsync(request.BrandFilterDto, cancellationToken);
    }
}
