using CarMarketApp.Application.DTOs.Models;
using CarMarketApp.Application.Models;
using CarMarketApp.Application.Models.ResultPattern;
using CarMarketApp.Domain.Entities;

namespace CarMarketApp.Application.Abstractions.Services;

public interface IModelService
{
    Task<Result> CreateModelAsync(CreateModelDto createModelDto, CancellationToken cancellationToken);
    Task<Result> UpdateModelAsync(UpdateModelDto updateModelDto, CancellationToken cancellationToken);
    Task<Result> DeleteModelAsync(Guid modelId, CancellationToken cancellationToken);
    Task<Result> RestoreModelAsync(Guid modelId, CancellationToken cancellationToken);
    Task<Result<PagedList<ModelDto>>> GetAllModelsAsync(ModelFilterDto modelFilterDto, CancellationToken cancellationToken);
}
