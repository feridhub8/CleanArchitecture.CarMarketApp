using FluentValidation;

namespace CarMarketApp.Application.Features.Commands.Models;

public sealed class DeleteModelCommandValidator : AbstractValidator<DeleteModelCommand>
{
    public DeleteModelCommandValidator()
    {
        RuleFor(m => m.ModelId).NotEmpty().WithMessage("Model id cannot be null");
    }
}
