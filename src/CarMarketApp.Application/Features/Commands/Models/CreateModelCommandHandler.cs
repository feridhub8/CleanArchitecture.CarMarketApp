using CarMarketApp.Application.Abstractions.Services;
using CarMarketApp.Application.Extensions;
using CarMarketApp.Application.Models.ResultPattern;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace CarMarketApp.Application.Features.Commands.Models;

public sealed class CreateModelCommandHandler : IRequestHandler<CreateModelCommand, Result>
{
    private readonly IValidator<CreateModelCommand> _validator;
    private readonly IModelService _modelService;

    public CreateModelCommandHandler(IValidator<CreateModelCommand> validator, IModelService modelService)
    {
        _validator = validator;
        _modelService = modelService;
    }

    public async Task<Result> Handle(CreateModelCommand request, CancellationToken cancellationToken)
    {
        ValidationResult validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return Result.Fail(null, validationResult.GetErrors());

        return await _modelService.CreateModelAsync(request.CreateModelDto, cancellationToken);
    }
}
