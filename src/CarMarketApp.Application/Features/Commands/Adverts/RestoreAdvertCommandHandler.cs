using CarMarketApp.Application.Abstractions.Services;
using CarMarketApp.Application.Extensions;
using CarMarketApp.Application.Models.ResultPattern;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace CarMarketApp.Application.Features.Commands.Adverts;

public sealed class RestoreAdvertCommandHandler : IRequestHandler<RestoreAdvertCommand, Result>
{
    private readonly IAdvertService _advertService;
    private readonly IValidator<RestoreAdvertCommand> _validator;

    public RestoreAdvertCommandHandler(IAdvertService advertService, IValidator<RestoreAdvertCommand> validator)
    {
        _advertService = advertService;
        _validator = validator;
    }

    public async Task<Result> Handle(RestoreAdvertCommand request, CancellationToken cancellationToken)
    {
        ValidationResult validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return Result.Fail(null, validationResult.GetErrors());

        return await _advertService.RestoreAdvertAsync(request.AdvertId, cancellationToken);
    }
}
