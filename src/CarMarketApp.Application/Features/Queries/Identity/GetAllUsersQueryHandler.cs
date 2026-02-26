using CarMarketApp.Application.Abstractions.Identity;
using CarMarketApp.Application.DTOs.Identity;
using CarMarketApp.Application.Models;
using CarMarketApp.Application.Models.ResultPattern;
using MediatR;

namespace CarMarketApp.Application.Features.Queries.Identity;

public sealed class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, Result<PagedList<UserDto>>>
{
    private readonly IUserService _userService;

    public GetAllUsersQueryHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<Result<PagedList<UserDto>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        return await _userService.GetAllUsersAsync(request.UserFilterDto, cancellationToken);
    }
}
