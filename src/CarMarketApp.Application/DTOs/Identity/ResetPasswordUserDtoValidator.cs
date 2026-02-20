using FluentValidation;

namespace CarMarketApp.Application.DTOs.Identity;

public sealed class ResetPasswordUserDtoValidator : AbstractValidator<ResetPasswordUserDto>
{
    public ResetPasswordUserDtoValidator()
    {
        RuleFor(u => u.Email).NotEmpty().WithMessage("Email cannot be empty")
                                 .EmailAddress().WithMessage("Invalid email address");

        RuleFor(u => u.NewPassword).NotEmpty().WithMessage("New password cannot be empty")
                                .MinimumLength(8).WithMessage("New password must be at least 8 characters long")
                                .MaximumLength(30).WithMessage("New password must be at most 30 characters long")
                                .Matches(@"^(?=.*[A-Z])(?=.*\d).+$")
                                .WithMessage("New password must contain at least one uppercase letter and one digit");
    }
}
