using CarMarketApp.Application.DTOs.Models;
using FluentValidation;

namespace CarMarketApp.Application.Features.Commands.Models;

public sealed class UpdateModelCommandValidator : AbstractValidator<UpdateModelCommand>
{
    public UpdateModelCommandValidator()
    {
        RuleFor(m => m.UpdateModelDto).SetValidator(new UpdateModelDtoValidator());
    }
}
