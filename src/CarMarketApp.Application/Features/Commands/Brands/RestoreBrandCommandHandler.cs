using CarMarketApp.Application.Abstractions.Services;
using CarMarketApp.Application.Models.ResultPattern;
using MediatR;

namespace CarMarketApp.Application.Features.Commands.Brands;

public sealed class RestoreBrandCommandHandler : IRequestHandler<RestoreBrandCommand, Result>
{
    private readonly IBrandService _brandService;

    public RestoreBrandCommandHandler(IBrandService brandService)
    {
        _brandService = brandService;
    }

    public async Task<Result> Handle(RestoreBrandCommand request, CancellationToken cancellationToken)
    {
        return await _brandService.RestoreBrandAsync(request.Id, cancellationToken);
    }
}
