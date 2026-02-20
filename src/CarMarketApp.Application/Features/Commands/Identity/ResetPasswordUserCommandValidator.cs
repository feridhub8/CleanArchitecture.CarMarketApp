using CarMarketApp.Application.DTOs.Identity;
using FluentValidation;

namespace CarMarketApp.Application.Features.Commands.Identity;

public sealed class ResetPasswordUserCommandValidator : AbstractValidator<ResetPasswordUserCommand>
{
    public ResetPasswordUserCommandValidator()
    {
        RuleFor(x => x.ResetPasswordUserDto).SetValidator(new ResetPasswordUserDtoValidator());
    }
}
