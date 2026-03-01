using FluentValidation;

namespace CarMarketApp.Application.DTOs.Models;

public sealed class UpdateModelDtoValidator : AbstractValidator<UpdateModelDto>
{
    public UpdateModelDtoValidator()
    {
        RuleFor(b => b.Id).NotEmpty().WithMessage("Brand id cannot be empty");

        RuleFor(b => b.Name).NotEmpty().WithMessage("Model name cannot be empty")
                            .MinimumLength(2).WithMessage("Model name must be at least 2 characters long")
                            .MaximumLength(20).WithMessage("Model name must be at most 20 characters long");

        RuleFor(b => b.BrandId).NotEmpty().WithMessage("Brand id cannot be empty");
    }
}
