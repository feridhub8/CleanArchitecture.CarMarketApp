using CarMarketApp.Application.Abstractions.Services;
using CarMarketApp.Application.Extensions;
using CarMarketApp.Application.Models.ResultPattern;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace CarMarketApp.Application.Features.Commands.Models;

public sealed class UpdateModelCommandHandler : IRequestHandler<UpdateModelCommand, Result>
{
    private readonly IValidator<UpdateModelCommand> _validator;
    private readonly IModelService _modelService;

    public UpdateModelCommandHandler(IValidator<UpdateModelCommand> validator, IModelService modelService)
    {
        _validator = validator;
        _modelService = modelService;
    }

    public async Task<Result> Handle(UpdateModelCommand request, CancellationToken cancellationToken)
    {
        ValidationResult validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return Result.Fail(null, validationResult.GetErrors());

        return await _modelService.UpdateModelAsync(request.UpdateModelDto, cancellationToken);
    }
}
