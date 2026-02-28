using AutoMapper;
using AutoMapper.QueryableExtensions;
using CarMarketApp.Application.Abstractions.Services;
using CarMarketApp.Application.Abstractions.UnitOfWork;
using CarMarketApp.Application.DTOs.Brands;
using CarMarketApp.Application.Models;
using CarMarketApp.Application.Models.ResultPattern;
using CarMarketApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarMarketApp.Infrastructure.Implementations.Services;

public sealed class BrandService : IBrandService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public BrandService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result> CreateBrandAsync(CreateBrandDto createBrandDto, CancellationToken cancellationToken)
    {

        Brand brand = _mapper.Map<Brand>(createBrandDto);
        brand.NormalizedName = brand.Normalize(brand.Name);

        Brand? existingBrand = await _unitOfWork.Brands.GetBrandByNameAsync(brand.NormalizedName, cancellationToken);

        if (existingBrand is not null)
            return Result.Fail($"Brand '{brand.Name}' already exists");

        _unitOfWork.Brands.Add(brand);

        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException)
        {
            return Result.Fail("Something went wrong while saving changes");
        }

        return Result.Ok($"Brand '{brand.Name}' has created successfully");
    }

    public async Task<Result> UpdateBrandAsync(UpdateBrandDto updateBrandDto, CancellationToken cancellationToken)
    {
        Brand? existingBrand = await _unitOfWork.Brands.GetBrandByIdAsync(updateBrandDto.Id, cancellationToken);

        if (existingBrand is null)
            return Result.Fail($"Brand not found");

        string normalizedName = existingBrand.Normalize(updateBrandDto.Name);
        Brand? duplicateBrand = await _unitOfWork.Brands.GetBrandByNameAsync(normalizedName, cancellationToken);
        if (duplicateBrand is not null && duplicateBrand.Id != updateBrandDto.Id)
            return Result.Fail($"Another brand with the name '{updateBrandDto.Name}' already exists");

        existingBrand.Name = updateBrandDto.Name;
        existingBrand.NormalizedName = existingBrand.Normalize(existingBrand.Name);
        existingBrand.UpdatedAt = DateTimeOffset.UtcNow;

        _unitOfWork.Brands.Update(existingBrand);

        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException)
        {
            return Result.Fail("Something went wrong while saving changes");
        }

        return Result.Ok($"Brand '{existingBrand.Name}' has been updated successfully");
    }

    public async Task<Result> DeleteBrandAsync(Guid brandId, CancellationToken cancellationToken)
    {
        Brand? existingBrand = await _unitOfWork.Brands.GetBrandByIdIgnoringQueryFilterAsync(brandId, cancellationToken);

        if (existingBrand is null)
            return Result.Fail("Brand not found");

        if (existingBrand.IsDeleted is true)
            return Result.Fail("Brand has already been deleted");

        existingBrand.IsDeleted = true;
        existingBrand.DeletedAt = DateTimeOffset.UtcNow;

        _unitOfWork.Brands.Update(existingBrand);

        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException)
        {
            return Result.Fail("Something went wrong while saving changes");
        }

        return Result.Ok($"Brand '{existingBrand.Name}' has been deleted successfully");
    }

    public async Task<Result> RestoreBrandAsync(Guid brandId, CancellationToken cancellationToken)
    {
        Brand? existingBrand = await _unitOfWork.Brands.GetBrandByIdIgnoringQueryFilterAsync(brandId, cancellationToken);

        if (existingBrand is null)
            return Result.Fail("Brand not found");

        if (!existingBrand.IsDeleted)
            return Result.Fail("Brand is not deleted");

        existingBrand.IsDeleted = false;
        existingBrand.DeletedAt = null;

        _unitOfWork.Brands.Update(existingBrand);

        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException)
        {
            return Result.Fail("Something went wrong while saving changes");
        }

        return Result.Ok($"Brand '{existingBrand.Name}' has been restored successfully");
    }

    public async Task<Result<PagedList<BrandDto>>> GetAllBrandsAsync(BrandFilterDto brandFilterDto, CancellationToken cancellationToken)
    {
        int page = brandFilterDto.Page;
        int pageSize = brandFilterDto.PageSize;
        string? search = brandFilterDto.Search;

        page = page < 1 ? 1 : page;
        pageSize = pageSize < 1 ? 10 : pageSize;
        pageSize = Math.Min(pageSize, 50);

        IQueryable<Brand> query = _unitOfWork.Brands.GetAllBrandsQuery().AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
        {
            string normalizedSearch = search.Trim().ToUpper();
            query = query.Where(b => b.NormalizedName.Contains(normalizedSearch));
        }

        int total = await query.CountAsync(cancellationToken);

        List<BrandDto> brands = await query
                    .OrderByDescending(b => b.CreatedAt)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ProjectTo<BrandDto>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);

        PagedList<BrandDto> pagedList = new(brands, total, page, pageSize);

        return Result<PagedList<BrandDto>>.Ok(pagedList);
    }
}
