using FluentValidation;

namespace CarMarketApp.Application.DTOs.Identity;

public sealed class ForgotPasswordUserDtoValidator : AbstractValidator<ForgotPasswordUserDto>
{
    public ForgotPasswordUserDtoValidator()
    {
        RuleFor(u => u.Email).NotEmpty().WithMessage("Email cannot be empty")
                                 .EmailAddress().WithMessage("Invalid email address");
    }
}
