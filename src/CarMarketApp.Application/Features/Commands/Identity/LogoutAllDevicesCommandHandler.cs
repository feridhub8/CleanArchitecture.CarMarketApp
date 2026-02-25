using CarMarketApp.Application.Abstractions.Identity;
using CarMarketApp.Application.Models.ResultPattern;
using MediatR;

namespace CarMarketApp.Application.Features.Commands.Identity;

public sealed class LogoutAllDevicesCommandHandler : IRequestHandler<LogoutAllDevicesCommand, Result>
{
    private readonly IUserService _userService;

    public LogoutAllDevicesCommandHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<Result> Handle(LogoutAllDevicesCommand request, CancellationToken cancellationToken)
    {
        return await _userService.LogoutAllDevicesAsync(request.UserId, cancellationToken);
    }
}
