using CarMarketApp.Application.DTOs.Identity;
using FluentValidation;

namespace CarMarketApp.Application.Features.Commands.Identity;

public sealed class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(u => u.UpdateUserDto).SetValidator(new UpdateUserDtoValidator());
    }
}
