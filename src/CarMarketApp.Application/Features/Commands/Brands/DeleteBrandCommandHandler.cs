using CarMarketApp.Application.Abstractions.Services;
using CarMarketApp.Application.Models.ResultPattern;
using MediatR;

namespace CarMarketApp.Application.Features.Commands.Brands;

public sealed class DeleteBrandCommandHandler : IRequestHandler<DeleteBrandCommand, Result>
{
    private readonly IBrandService _brandService;

    public DeleteBrandCommandHandler(IBrandService brandService)
    {
        _brandService = brandService;
    }

    public async Task<Result> Handle(DeleteBrandCommand request, CancellationToken cancellationToken)
    {
        return await _brandService.DeleteBrandAsync(request.Id, cancellationToken);
    }
}
