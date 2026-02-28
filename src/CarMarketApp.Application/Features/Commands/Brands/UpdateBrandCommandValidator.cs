using CarMarketApp.Application.DTOs.Brands;
using FluentValidation;

namespace CarMarketApp.Application.Features.Commands.Brands;

public sealed class UpdateBrandCommandValidator : AbstractValidator<UpdateBrandCommand>
{
    public UpdateBrandCommandValidator()
    {
        RuleFor(b => b.UpdateBrandDto).SetValidator(new UpdateBrandDtoValidator());
    }
}
