using CarMarketApp.Application.DTOs.Identity;
using CarMarketApp.Application.DTOs.Users;
using CarMarketApp.Application.Models;
using CarMarketApp.Application.Models.ResultPattern;

namespace CarMarketApp.Application.Abstractions.Identity;

public interface IUserService
{
    Task<Result> RegisterUserAsync(RegisterUserDto registerUserDto);
    Task<Result<LoginResponse>> LoginUserAsync(LoginUserDto loginUserDto, CancellationToken cancellationToken);
    Task<Result<LoginResponse>> RefreshTokenAsync(RefreshTokenDto refreshTokenDto, CancellationToken cancellationToken);
}
