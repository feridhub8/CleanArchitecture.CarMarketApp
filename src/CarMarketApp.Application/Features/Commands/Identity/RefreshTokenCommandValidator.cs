using CarMarketApp.Application.DTOs.Identity;
using FluentValidation;

namespace CarMarketApp.Application.Features.Commands.Identity;

public sealed class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenCommandValidator()
    {
        RuleFor(r => r.RefreshTokenDto).SetValidator(new RefreshTokenDtoValidator());
    }
}
