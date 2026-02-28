using FluentValidation;

namespace CarMarketApp.Application.DTOs.Brands;

public sealed class CreateBrandDtoValidator : AbstractValidator<CreateBrandDto>
{
    public CreateBrandDtoValidator()
    {
        RuleFor(b => b.Name).NotEmpty().WithMessage("Brand name cannot be empty")
                            .MinimumLength(2).WithMessage("Brand name must be at least 2 characters long")
                            .MaximumLength(20).WithMessage("Brand name must be at most 20 characters long");
    }
}
