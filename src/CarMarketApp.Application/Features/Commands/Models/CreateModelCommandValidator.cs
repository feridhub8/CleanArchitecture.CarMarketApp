using CarMarketApp.Application.DTOs.Models;
using FluentValidation;

namespace CarMarketApp.Application.Features.Commands.Models;

public sealed class CreateModelCommandValidator : AbstractValidator<CreateModelCommand>
{
    public CreateModelCommandValidator()
    {
        RuleFor(m => m.CreateModelDto).SetValidator(new CreateModelDtoValidator());
    }
}
