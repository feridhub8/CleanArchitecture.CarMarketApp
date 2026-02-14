using AutoMapper;
using CarMarketApp.Application.Abstractions;
using CarMarketApp.Application.DTOs.Users;
using CarMarketApp.Application.Models.ResultPattern;
using CarMarketApp.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;

namespace CarMarketApp.Infrastructure.Implementations.Services;

public sealed class UserService : IUserService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IMapper _mapper;

    public UserService(UserManager<AppUser> userManager, IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<Result> RegisterUserAsync(RegisterUserDto registerUserDto)
    {
        AppUser user = _mapper.Map<AppUser>(registerUserDto);

        IdentityResult result = await _userManager.CreateAsync(user, registerUserDto.Password);

        if (!result.Succeeded)
            return Result.Fail(null, result.Errors
                                           .GroupBy(e => e.Code)
                                           .ToDictionary(
                                                g => g.Key,
                                                g => g.Select(x => x.Description).ToArray()));

        return Result.Ok("User has successfully registered");
    }
}
