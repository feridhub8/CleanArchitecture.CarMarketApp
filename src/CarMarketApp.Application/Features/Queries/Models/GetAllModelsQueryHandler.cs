using CarMarketApp.Application.Abstractions.Services;
using CarMarketApp.Application.DTOs.Models;
using CarMarketApp.Application.Models;
using CarMarketApp.Application.Models.ResultPattern;
using MediatR;

namespace CarMarketApp.Application.Features.Queries.Models;

public sealed class GetAllModelsQueryHandler : IRequestHandler<GetAllModelsQuery, Result<PagedList<ModelDto>>>
{
    private readonly IModelService _modelService;

    public GetAllModelsQueryHandler(IModelService modelService)
    {
        _modelService = modelService;
    }

    public async Task<Result<PagedList<ModelDto>>> Handle(GetAllModelsQuery request, CancellationToken cancellationToken)
    {
        return await _modelService.GetAllModelsAsync(request.ModelFilterDto, cancellationToken);
    }
}
