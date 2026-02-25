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
    Task<Result> LogoutCurrentDeviceAsync(LogoutCurrentDeviceDto logoutCurrentDeviceDto, CancellationToken cancellationToken);
    Task<Result> LogoutAllDevicesAsync(Guid userId, CancellationToken cancellationToken);
    Task<Result> ForgotPasswordAsync(ForgotPasswordUserDto forgotPasswordUserDto);
    Task<Result> ResetPasswordAsync(ResetPasswordUserDto resetPasswordUserDto, string resetToken);
    Task<Result> UpdateUserAsync(Guid userId, UpdateUserDto updateUserDto);
    Task<Result> AddModeratorAsync(Guid userId);
    Task<Result> RemoveModeratorAsync(Guid userId);
    Task<Result> DeleteUserAsync(Guid userId);
    Task<Result> RestoreUserAsync(Guid userId);
    Task<Result<PagedList<UserDto>>> GetAllUsersAsync(int page, int pageSize, CancellationToken cancellationToken);
}
