using CarMarketApp.Application.DTOs.Adverts;
using FluentValidation;

namespace CarMarketApp.Application.Features.Commands.Adverts;

public sealed class CreateAdvertCommandValidator : AbstractValidator<CreateAdvertCommand>
{
    public CreateAdvertCommandValidator()
    {
        RuleFor(a => a.CreateAdvertDto).SetValidator(new CreateAdvertDtoValidator());

        RuleFor(a => a.UserId).NotEmpty().WithMessage("User id cannot be empty");
    }
}
