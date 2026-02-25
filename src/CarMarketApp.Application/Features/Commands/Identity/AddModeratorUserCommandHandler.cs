using CarMarketApp.Application.Abstractions.Identity;
using CarMarketApp.Application.Models.ResultPattern;
using MediatR;

namespace CarMarketApp.Application.Features.Commands.Identity;

public sealed class AddModeratorUserCommandHandler : IRequestHandler<AddModeratorUserCommand, Result>
{
    private readonly IUserService _userService;

    public AddModeratorUserCommandHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<Result> Handle(AddModeratorUserCommand request, CancellationToken cancellationToken)
    {
        return await _userService.AddModeratorAsync(request.UserId);
    }
}
