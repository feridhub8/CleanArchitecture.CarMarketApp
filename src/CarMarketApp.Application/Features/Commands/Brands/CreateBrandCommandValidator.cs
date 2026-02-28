using CarMarketApp.Application.DTOs.Brands;
using FluentValidation;

namespace CarMarketApp.Application.Features.Commands.Brands;

public sealed class CreateBrandCommandValidator : AbstractValidator<CreateBrandCommand>
{
    public CreateBrandCommandValidator()
    {
        RuleFor(b => b.CreateBrandDto).SetValidator(new CreateBrandDtoValidator());
    }
}
