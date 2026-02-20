using CarMarketApp.Application.DTOs.Identity;
using FluentValidation;

namespace CarMarketApp.Application.Features.Commands.Identity;

public sealed class ForgotPasswordUserCommandValidator : AbstractValidator<ForgotPasswordUserCommand>
{
    public ForgotPasswordUserCommandValidator()
    {
        RuleFor(u => u.ForgotPasswordUserDto).SetValidator(new ForgotPasswordUserDtoValidator());
    }
}
