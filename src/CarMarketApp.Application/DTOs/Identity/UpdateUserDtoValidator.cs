using FluentValidation;

namespace CarMarketApp.Application.DTOs.Identity;

public sealed class UpdateUserDtoValidator : AbstractValidator<UpdateUserDto>
{
    public UpdateUserDtoValidator()
    {
        RuleFor(u => u.FirstName).NotEmpty().WithMessage("First name cannot be empty")
                                 .MinimumLength(2).WithMessage("First name must be at least 2 characters long")
                                 .MaximumLength(20).WithMessage("First name must be at most 20 characters long");

        RuleFor(u => u.LastName).NotEmpty().WithMessage("Last name cannot be empty")
                                 .MinimumLength(2).WithMessage("Last name must be at least 2 characters long")
                                 .MaximumLength(20).WithMessage("Last name must be at most 20 characters long");
    }
}
