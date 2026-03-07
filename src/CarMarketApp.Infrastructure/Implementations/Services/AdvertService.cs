using AutoMapper;
using AutoMapper.QueryableExtensions;
using CarMarketApp.Application.Abstractions.Services;
using CarMarketApp.Application.Abstractions.UnitOfWork;
using CarMarketApp.Application.DTOs.Adverts;
using CarMarketApp.Application.DTOs.Models;
using CarMarketApp.Application.Models;
using CarMarketApp.Application.Models.ResultPattern;
using CarMarketApp.Domain.Entities;
using CarMarketApp.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CarMarketApp.Infrastructure.Implementations.Services;

public sealed class AdvertService : IAdvertService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly UserManager<AppUser> _userManager;

    public AdvertService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<AppUser> userManager)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _userManager = userManager;
    }

    public async Task<Result> CreateAdvertAsync(CreateAdvertDto createAdvertDto, Guid userId, CancellationToken cancellationToken)
    {
        AppUser? appUser = await _userManager.FindByIdAsync(userId.ToString());

        if (appUser is null)
            return Result.Fail("User not found");

        Advert advert = _mapper.Map<Advert>(createAdvertDto);

        advert.UserId = userId;

        _unitOfWork.Adverts.Add(advert);

        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException)
        {
            return Result.Fail("Something went wrong while saving changes");
        }

        return Result.Ok("Advert has created successfully");
    }

    public async Task<Result> UpdateAdvertAsync(UpdateAdvertDto updateAdvertDto, CancellationToken cancellationToken)
    {
        Advert? existingAdvert = await _unitOfWork.Adverts.GetAdvertByIdIgnoringQueryFilterAsync(updateAdvertDto.Id, cancellationToken);

        if (existingAdvert is null)
            return Result.Fail("Advert not found");

        _mapper.Map(updateAdvertDto, existingAdvert);
        existingAdvert.UpdatedAt = DateTimeOffset.UtcNow;
        _unitOfWork.Adverts.Update(existingAdvert);

        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException)
        {
            return Result.Fail("Something went wrong while saving changes");
        }

        return Result.Ok("Advert has updated successfully");
    }

    public async Task<Result> DeleteAdvertAsync(Guid advertId, CancellationToken cancellationToken)
    {
        Advert? existingAdvert = await _unitOfWork.Adverts.GetAdvertByIdIgnoringQueryFilterAsync(advertId, cancellationToken);

        if (existingAdvert is null)
            return Result.Fail("Advert not found");

        if (existingAdvert.IsDeleted == true)
            return Result.Fail("Advert has already deleted");

        existingAdvert.IsDeleted = true;
        existingAdvert.DeletedAt = DateTimeOffset.UtcNow;

        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException)
        {
            return Result.Fail("Something went wrong while saving changes");
        }

        return Result.Ok("Advert has been deleted successfully");
    }

    public async Task<Result> RestoreAdvertAsync(Guid advertId, CancellationToken cancellationToken)
    {
        Advert? existingAdvert = await _unitOfWork.Adverts.GetAdvertByIdIgnoringQueryFilterAsync(advertId, cancellationToken);

        if (existingAdvert is null)
            return Result.Fail("Advert not found");

        if (!existingAdvert.IsDeleted)
            return Result.Fail("Advert is not deleted");

        existingAdvert.IsDeleted = false;
        existingAdvert.DeletedAt = null;

        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException)
        {
            return Result.Fail("Something went wrong while saving changes");
        }

        return Result.Ok("Advert has been restored successfully");
    }

    public async Task<Result<PagedList<AdvertDto>>> GetAllAdvertsAsync(AdvertFilterDto advertFilterDto, CancellationToken cancellationToken)
    {
        int page = advertFilterDto.Page;
        int pageSize = advertFilterDto.PageSize;
        Guid? brandId = advertFilterDto.BrandId;
        Guid? modelId = advertFilterDto.ModelId;

        page = page < 1 ? 1 : page;
        pageSize = pageSize < 1 ? 10 : pageSize;
        pageSize = Math.Min(pageSize, 50);

        IQueryable<Advert> query = _unitOfWork.Adverts.GetAllAdvertsQuery();

        if (modelId is not null)
        {
            query = query.Where(a => a.ModelId == modelId);
        }

        if (brandId is not null)
        {
            query = query.Where(a => a.Model.BrandId == brandId);
        }

        int total = await query.CountAsync(cancellationToken);

        List<AdvertDto> adverts = await query
                    .OrderByDescending(b => b.CreatedAt)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ProjectTo<AdvertDto>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);

        PagedList<AdvertDto> pagedList = new(adverts, total, page, pageSize);

        return Result<PagedList<AdvertDto>>.Ok(pagedList);
    }
}
