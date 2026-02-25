using CarMarketApp.Application.Abstractions.Identity;
using CarMarketApp.Application.Models.ResultPattern;
using MediatR;

namespace CarMarketApp.Application.Features.Commands.Identity;

public sealed class RestoreUserCommandHandler : IRequestHandler<RestoreUserCommand, Result>
{
    private readonly IUserService _userService;

    public RestoreUserCommandHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<Result> Handle(RestoreUserCommand request, CancellationToken cancellationToken)
    {
        return await _userService.RestoreUserAsync(request.UserId);
    }
}
