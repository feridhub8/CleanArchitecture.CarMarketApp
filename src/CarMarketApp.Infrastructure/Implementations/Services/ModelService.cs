using AutoMapper;
using AutoMapper.QueryableExtensions;
using CarMarketApp.Application.Abstractions.Services;
using CarMarketApp.Application.Abstractions.UnitOfWork;
using CarMarketApp.Application.DTOs.Models;
using CarMarketApp.Application.Models;
using CarMarketApp.Application.Models.ResultPattern;
using CarMarketApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarMarketApp.Infrastructure.Implementations.Services;

public sealed class ModelService : IModelService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public ModelService(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> CreateModelAsync(CreateModelDto createModelDto, CancellationToken cancellationToken)
    {
        Model model = _mapper.Map<Model>(createModelDto);
        model.NormalizedName = model.Normalize(model.Name);

        Model? existingModel = await _unitOfWork.Models.GetModelByNameIgnoringQueryFilterAsync(model.NormalizedName, model.BrandId, cancellationToken);

        if (existingModel is not null)
            return Result.Fail($"Model '{model.Name}' already exists");

        _unitOfWork.Models.Add(model);

        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException)
        {
            return Result.Fail("Something went wrong while saving changes");
        }

        return Result.Ok($"Model '{model.Name}' has created successfully");
    }

    public async Task<Result> UpdateModelAsync(UpdateModelDto updateModelDto, CancellationToken cancellationToken)
    {
        Model? existingModel = await _unitOfWork.Models.GetModelByIdIgnoringQueryFilterAsync(updateModelDto.Id, cancellationToken);

        if (existingModel is null)
            return Result.Fail($"Model not found");

        string normalizedName = existingModel.Normalize(updateModelDto.Name);

        Model? duplicateModel = await _unitOfWork.Models.GetModelByNameIgnoringQueryFilterAsync(normalizedName, updateModelDto.BrandId, cancellationToken);

        if (duplicateModel is not null && duplicateModel.Id != updateModelDto.Id)
            return Result.Fail($"Another model with the name '{updateModelDto.Name}' already exists");

        existingModel.Name = updateModelDto.Name;
        existingModel.NormalizedName = normalizedName;
        existingModel.BrandId = updateModelDto.BrandId;
        existingModel.UpdatedAt = DateTimeOffset.UtcNow;

        _unitOfWork.Models.Update(existingModel);

        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException)
        {
            return Result.Fail("Something went wrong while saving changes");
        }

        return Result.Ok($"Model '{existingModel.Name}' has updated successfully");
    }

    public async Task<Result> DeleteModelAsync(Guid modelId, CancellationToken cancellationToken)
    {
        Model? existingModel = await _unitOfWork.Models.GetModelByIdIgnoringQueryFilterAsync(modelId, cancellationToken);

        if (existingModel is null)
            return Result.Fail("Model not found");

        if (existingModel.IsDeleted is true)
            return Result.Fail("Model has already been deleted");

        existingModel.IsDeleted = true;
        existingModel.DeletedAt = DateTimeOffset.UtcNow;

        _unitOfWork.Models.Update(existingModel);

        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException)
        {
            return Result.Fail("Something went wrong while saving changes");
        }

        return Result.Ok($"Model '{existingModel.Name}' has been deleted successfully");
    }

    public async Task<Result> RestoreModelAsync(Guid modelId, CancellationToken cancellationToken)
    {
        Model? existingModel = await _unitOfWork.Models.GetModelByIdIgnoringQueryFilterAsync(modelId, cancellationToken);

        if (existingModel is null)
            return Result.Fail("Model not found");

        if (!existingModel.IsDeleted)
            return Result.Fail("Model is not deleted");

        existingModel.IsDeleted = false;
        existingModel.DeletedAt = null;

        _unitOfWork.Models.Update(existingModel);

        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException)
        {
            return Result.Fail("Something went wrong while saving changes");
        }

        return Result.Ok($"Model '{existingModel.Name}' has been restored successfully");
    }

    public async Task<Result<PagedList<ModelDto>>> GetAllModelsAsync(ModelFilterDto modelFilterDto, CancellationToken cancellationToken)
    {
        int page = modelFilterDto.Page;
        int pageSize = modelFilterDto.PageSize;
        string? search = modelFilterDto.Search;

        page = page < 1 ? 1 : page;
        pageSize = pageSize < 1 ? 10 : pageSize;
        pageSize = Math.Min(pageSize, 50);

        IQueryable<Model> query = _unitOfWork.Models.GetAllModelsQuery()
                                                    .Include(m => m.Brand)
                                                    .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
        {
            string normalizedSearch = search.Trim().ToUpper();
            query = query.Where(b => b.NormalizedName.Contains(normalizedSearch));
        }

        int total = await query.CountAsync(cancellationToken);

        List<ModelDto> models = await query
                    .OrderByDescending(b => b.CreatedAt)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ProjectTo<ModelDto>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);

        PagedList<ModelDto> pagedList = new(models, total, page, pageSize);

        return Result<PagedList<ModelDto>>.Ok(pagedList);
    }
}
