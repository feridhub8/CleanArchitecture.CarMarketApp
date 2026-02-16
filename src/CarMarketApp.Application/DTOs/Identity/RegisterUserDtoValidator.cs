using FluentValidation;

namespace CarMarketApp.Application.DTOs.Users;

public sealed class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto>
{
    public RegisterUserDtoValidator()
    {
        RuleFor(u => u.FirstName).NotEmpty().WithMessage("First name cannot be empty")
                                 .MinimumLength(2).WithMessage("First name must be at least 2 characters long")
                                 .MaximumLength(20).WithMessage("First name must be at most 20 characters long");

        RuleFor(u => u.LastName).NotEmpty().WithMessage("Last name cannot be empty")
                                 .MinimumLength(2).WithMessage("Last name must be at least 2 characters long")
                                 .MaximumLength(20).WithMessage("Last name must be at most 20 characters long");

        RuleFor(u => u.UserName).NotEmpty().WithMessage("First name cannot be empty")
                                 .MinimumLength(2).WithMessage("First name must be at least 2 characters long")
                                 .MaximumLength(20).WithMessage("First name must be at most 20 characters long");

        RuleFor(u => u.Email).NotEmpty().WithMessage("Email cannot be empty")
                                 .EmailAddress().WithMessage("Invalid email address");

        RuleFor(u => u.Password).NotEmpty().WithMessage("Password cannot be empty")
                                .MinimumLength(8).WithMessage("Password must be at least 8 characters long")
                                .MaximumLength(30).WithMessage("Password must be at most 30 characters long")
                                .Matches(@"^(?=.*[A-Z])(?=.*\d).+$")
                                .WithMessage("Password must contain at least one uppercase letter and one digit");
    }
}
