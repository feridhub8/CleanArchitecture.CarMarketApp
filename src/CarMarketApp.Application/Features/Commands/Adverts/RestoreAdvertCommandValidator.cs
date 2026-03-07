using FluentValidation;

namespace CarMarketApp.Application.Features.Commands.Adverts;

public sealed class RestoreAdvertCommandValidator : AbstractValidator<RestoreAdvertCommand>
{
    public RestoreAdvertCommandValidator()
    {
        RuleFor(a => a.AdvertId).NotEmpty().WithMessage("Id cannot be null");
    }
}
