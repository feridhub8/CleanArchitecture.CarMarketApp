using CarMarketApp.Application.Abstractions.Identity;
using CarMarketApp.Application.Extensions;
using CarMarketApp.Application.Models;
using CarMarketApp.Application.Models.ResultPattern;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace CarMarketApp.Application.Features.Commands.Identity;

public sealed class LogoutCurrentDeviceCommandHandler : IRequestHandler<LogoutCurrentDeviceCommand, Result>
{
    private readonly IValidator<LogoutCurrentDeviceCommand> _validator;
    private readonly IUserService _userService;

    public LogoutCurrentDeviceCommandHandler(IValidator<LogoutCurrentDeviceCommand> validator, IUserService userService)
    {
        _validator = validator;
        _userService = userService;
    }

    public async Task<Result> Handle(LogoutCurrentDeviceCommand request, CancellationToken cancellationToken)
    {
        if (request is null)
            return Result.Fail("Request cannot be null");

        ValidationResult validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return Result.Fail("Validation Error", validationResult.GetErrors());

        return await _userService.LogoutCurrentDeviceAsync(request.LogoutCurrentDeviceDto, cancellationToken);
    }
}
