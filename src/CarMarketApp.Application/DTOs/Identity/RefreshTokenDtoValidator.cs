using FluentValidation;

namespace CarMarketApp.Application.DTOs.Identity;

public sealed class RefreshTokenDtoValidator : AbstractValidator<RefreshTokenDto>
{
    public RefreshTokenDtoValidator()
    {
        RuleFor(u => u.Token).NotEmpty().WithMessage("Refresh token cannot be empty");
    }
}
