using CarMarketApp.Application.Abstractions.Services;
using CarMarketApp.Application.Extensions;
using CarMarketApp.Application.Models.ResultPattern;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace CarMarketApp.Application.Features.Commands.Adverts;

public sealed class DeleteAdvertCommandHandler : IRequestHandler<DeleteAdvertCommand, Result>
{
    private readonly IAdvertService _advertService;
    private readonly IValidator<DeleteAdvertCommand> _validator;

    public DeleteAdvertCommandHandler(IAdvertService advertService, IValidator<DeleteAdvertCommand> validator)
    {
        _advertService = advertService;
        _validator = validator;
    }

    public async Task<Result> Handle(DeleteAdvertCommand request, CancellationToken cancellationToken)
    {
        ValidationResult validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return Result.Fail(null, validationResult.GetErrors());

        return await _advertService.DeleteAdvertAsync(request.AdvertId, cancellationToken);
    }
}
