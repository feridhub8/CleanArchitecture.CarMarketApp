using FluentValidation;

namespace CarMarketApp.Application.Features.Commands.Adverts;

public sealed class DeleteAdvertCommandValidator : AbstractValidator<DeleteAdvertCommand>
{
    public DeleteAdvertCommandValidator()
    {
        RuleFor(a => a.AdvertId).NotEmpty().WithMessage("Id cannot be null");
    }
}
