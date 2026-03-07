using CarMarketApp.Application.Abstractions.Services;
using CarMarketApp.Application.Extensions;
using CarMarketApp.Application.Models.ResultPattern;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace CarMarketApp.Application.Features.Commands.Adverts;

public sealed class CreateAdvertCommandHandler : IRequestHandler<CreateAdvertCommand, Result>
{
    private readonly IAdvertService _advertService;
    private readonly IValidator<CreateAdvertCommand> _validator;

    public CreateAdvertCommandHandler(IAdvertService advertService, IValidator<CreateAdvertCommand> validator)
    {
        _advertService = advertService;
        _validator = validator;
    }

    public async Task<Result> Handle(CreateAdvertCommand request, CancellationToken cancellationToken)
    {
        ValidationResult validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return Result.Fail(null, validationResult.GetErrors());

        return await _advertService.CreateAdvertAsync(request.CreateAdvertDto, request.UserId, cancellationToken);
    }
}
