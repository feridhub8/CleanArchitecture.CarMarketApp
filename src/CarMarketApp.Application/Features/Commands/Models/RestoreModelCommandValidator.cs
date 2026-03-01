using FluentValidation;

namespace CarMarketApp.Application.Features.Commands.Models;

public sealed class RestoreModelCommandValidator : AbstractValidator<RestoreModelCommand>
{
    public RestoreModelCommandValidator()
    {
        RuleFor(m => m.ModelId).NotEmpty().WithMessage("Model id cannot be null");
    }
}
