using CarMarketApp.Application.DTOs.Identity;
using FluentValidation;

namespace CarMarketApp.Application.Features.Commands.Identity;

public sealed class LogoutCurrentDeviceCommandValidator : AbstractValidator<LogoutCurrentDeviceCommand>
{
    public LogoutCurrentDeviceCommandValidator()
    {
        RuleFor(x => x.LogoutCurrentDeviceDto).SetValidator(new LogoutCurrentDeviceDtoValidator());
    }
}
