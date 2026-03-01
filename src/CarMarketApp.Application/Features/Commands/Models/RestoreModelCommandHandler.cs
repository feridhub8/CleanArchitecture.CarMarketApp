using CarMarketApp.Application.Abstractions.Services;
using CarMarketApp.Application.Extensions;
using CarMarketApp.Application.Models.ResultPattern;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace CarMarketApp.Application.Features.Commands.Models;

public sealed class RestoreModelCommandHandler
    : IRequestHandler<RestoreModelCommand, Result>
{
    private readonly IValidator<RestoreModelCommand> _validator;
    private readonly IModelService _modelService;

    public RestoreModelCommandHandler(IValidator<RestoreModelCommand> validator, IModelService modelService)
    {
        _validator = validator;
        _modelService = modelService;
    }

    public async Task<Result> Handle(RestoreModelCommand request, CancellationToken cancellationToken)
    {
        ValidationResult validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return Result.Fail(null, validationResult.GetErrors());

        return await _modelService.RestoreModelAsync(request.ModelId, cancellationToken);
    }
}
