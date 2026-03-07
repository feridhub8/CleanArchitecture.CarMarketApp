using CarMarketApp.Application.Abstractions.Services;
using CarMarketApp.Application.DTOs.Adverts;
using CarMarketApp.Application.Models;
using CarMarketApp.Application.Models.ResultPattern;
using MediatR;

namespace CarMarketApp.Application.Features.Queries.Adverts;

public sealed class GetAllAdvertsQueryHandler : IRequestHandler<GetAllAdvertsQuery, Result<PagedList<AdvertDto>>>
{
    private readonly IAdvertService _advertService;

    public GetAllAdvertsQueryHandler(IAdvertService advertService)
    {
        _advertService = advertService;
    }

    public async Task<Result<PagedList<AdvertDto>>> Handle(GetAllAdvertsQuery request, CancellationToken cancellationToken)
    {
        return await _advertService.GetAllAdvertsAsync(request.AdvertFilterDto, cancellationToken);
    }
}
