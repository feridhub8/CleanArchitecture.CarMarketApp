using CarMarketApp.Application.DTOs.Adverts;
using FluentValidation;

namespace CarMarketApp.Application.Features.Commands.Adverts;

public sealed class UpdateAdvertCommandValidator : AbstractValidator<UpdateAdvertCommand>
{
    public UpdateAdvertCommandValidator()
    {
        RuleFor(a => a.UpdateAdvertDto).SetValidator(new UpdateAdvertDtoValidator());
    }
}
