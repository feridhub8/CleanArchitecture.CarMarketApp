using FluentValidation;

namespace CarMarketApp.Application.DTOs.Identity;

public sealed class LogoutCurrentDeviceDtoValidator : AbstractValidator<LogoutCurrentDeviceDto>
{
    public LogoutCurrentDeviceDtoValidator()
    {
        RuleFor(u => u.Token).NotEmpty().WithMessage("Refresh token cannot be empty");
    }
}
