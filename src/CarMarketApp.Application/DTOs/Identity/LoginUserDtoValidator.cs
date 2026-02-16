using FluentValidation;

namespace CarMarketApp.Application.DTOs.Users;

public sealed class LoginUserDtoValidator : AbstractValidator<LoginUserDto>
{
    public LoginUserDtoValidator()
    {
        RuleFor(u => u.EmailOrUserName).NotEmpty().WithMessage("Email or username cannot be empty");

        RuleFor(u => u.Password).NotEmpty().WithMessage("Password cannot be empty")
                                .MinimumLength(8).WithMessage("Password must be at least 8 characters long")
                                .MaximumLength(30).WithMessage("Password must be at most 30 characters long");
    }
}
