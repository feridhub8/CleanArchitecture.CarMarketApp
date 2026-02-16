using CarMarketApp.Application.DTOs.Users;
using FluentValidation;

namespace CarMarketApp.Application.Features.Commands.Users;

public sealed class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(u => u.RegisterUserDto).SetValidator(new RegisterUserDtoValidator());
    }
}
