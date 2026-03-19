using CarMarketApp.Application.Abstractions.Services;
using CarMarketApp.Application.Extensions;
using CarMarketApp.Application.Models.ResultPattern;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace CarMarketApp.Application.Features.Commands.Adverts;

public sealed class UpdateAdvertCommandHandler : IRequestHandler<UpdateAdvertCommand, Result>
{
    private readonly IAdvertService _advertService;
    private readonly IValidator<UpdateAdvertCommand> _validator;

    public UpdateAdvertCommandHandler(IAdvertService advertService, IValidator<UpdateAdvertCommand> validator)
    {
        _advertService = advertService;
        _validator = validator;
    }

    public async Task<Result> Handle(UpdateAdvertCommand request, CancellationToken cancellationToken)
    {
        ValidationResult validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return Result.Fail(null, validationResult.GetErrors());

        return await _advertService.UpdateAdvertAsync(request.UpdateAdvertDto, request.UserId, cancellationToken);
    }
}
