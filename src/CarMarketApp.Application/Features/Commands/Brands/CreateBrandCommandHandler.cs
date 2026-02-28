using CarMarketApp.Application.Abstractions.Services;
using CarMarketApp.Application.Extensions;
using CarMarketApp.Application.Models.ResultPattern;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace CarMarketApp.Application.Features.Commands.Brands;

public sealed class CreateBrandCommandHandler : IRequestHandler<CreateBrandCommand, Result>
{
    private readonly IValidator<CreateBrandCommand> _validator;
    private readonly IBrandService _brandService;

    public CreateBrandCommandHandler(IValidator<CreateBrandCommand> validator, IBrandService brandService)
    {
        _validator = validator;
        _brandService = brandService;
    }

    public async Task<Result> Handle(CreateBrandCommand request, CancellationToken cancellationToken)
    {
        ValidationResult validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return Result.Fail(null, validationResult.GetErrors());

        return await _brandService.CreateBrandAsync(request.CreateBrandDto, cancellationToken);
    }
}
