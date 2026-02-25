using CarMarketApp.Application.Abstractions.Identity;
using CarMarketApp.Application.Models.ResultPattern;
using MediatR;

namespace CarMarketApp.Application.Features.Commands.Identity;

public sealed class RemoveModeratorUserCommandHandler : IRequestHandler<RemoveModeratorUserCommand, Result>
{
    private readonly IUserService _userService;

    public RemoveModeratorUserCommandHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<Result> Handle(RemoveModeratorUserCommand request, CancellationToken cancellationToken)
    {
        return await _userService.RemoveModeratorAsync(request.UserId);
    }
}
