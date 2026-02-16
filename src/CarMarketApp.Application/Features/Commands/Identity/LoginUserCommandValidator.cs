using CarMarketApp.Application.DTOs.Users;
using FluentValidation;

namespace CarMarketApp.Application.Features.Commands.Users;

public sealed class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidator()
    {
        RuleFor(u => u.LoginUserDto).SetValidator(new LoginUserDtoValidator());
    }
}
