using AutoMapper;
using Azure.Core;
using CarMarketApp.Application.Abstractions.Helpers;
using CarMarketApp.Application.Abstractions.Identity;
using CarMarketApp.Application.Abstractions.UnitOfWork;
using CarMarketApp.Application.DTOs.Identity;
using CarMarketApp.Application.DTOs.Users;
using CarMarketApp.Application.Models;
using CarMarketApp.Application.Models.ResultPattern;
using CarMarketApp.Domain.Entities;
using CarMarketApp.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Threading;

namespace CarMarketApp.Infrastructure.Implementations.Identity;

public sealed class UserService : IUserService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IMapper _mapper;
    private readonly ITokenGenerator _tokenGenerator;
    private readonly ITokenHasher _tokenHasher;
    private readonly IUnitOfWork _unitOfWork;

    public UserService(
        UserManager<AppUser> userManager,
        IMapper mapper,
        ITokenGenerator tokenGenerator,
        ITokenHasher tokenHasher,
        IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _mapper = mapper;
        _tokenGenerator = tokenGenerator;
        _tokenHasher = tokenHasher;
        _unitOfWork = unitOfWork;
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

        IdentityResult identityResult = await _userManager.AddToRoleAsync(user, "User");

        return identityResult.Succeeded
            ? Result.Ok("User has successfully registered")
            : Result.Fail("Something went wrong while adding role to user");
    }

    public async Task<Result<LoginResponse>> LoginUserAsync(LoginUserDto loginUserDto, CancellationToken cancellationToken)
    {
        AppUser? user = await _userManager.Users.FirstOrDefaultAsync(u => string.Equals(u.Email, loginUserDto.EmailOrUserName) || string.Equals(u.UserName, loginUserDto.EmailOrUserName));

        if (user is null)
            return Result<LoginResponse>.Fail("Email or username or password is invalid");

        bool loginResult = await _userManager.CheckPasswordAsync(user, loginUserDto.Password);

        if (!loginResult)
            return Result<LoginResponse>.Fail("Email or username or password is invalid");

        UserClaimsDto userClaimsDto = _mapper.Map<UserClaimsDto>(user);
        string accessToken = await _tokenGenerator.GenerateJwtToken(userClaimsDto);

        string plainRefreshToken = _tokenGenerator.GenerateToken();
        RefreshToken refreshToken = new RefreshToken
        {
            AppUserId = user.Id,
            TokenHash = _tokenHasher.HashToken(plainRefreshToken)
        };

        _unitOfWork.RefreshTokens.Add(refreshToken);

        int changes = await _unitOfWork.SaveChangesAsync(cancellationToken);

        return changes == 0
            ? Result<LoginResponse>.Fail("Something went wrong while saving changes")
            : Result<LoginResponse>.Ok(new LoginResponse(user.UserName!, accessToken, plainRefreshToken), "User has logged in successfully");
    }

    public async Task<Result<LoginResponse>> RefreshTokenAsync(RefreshTokenDto refreshTokenDto, CancellationToken cancellationToken)
    {
        string tokenHash = _tokenHasher.HashToken(refreshTokenDto.Token);

        RefreshToken? refreshToken = await _unitOfWork.RefreshTokens.GetRefreshTokenByToken(tokenHash, cancellationToken);

        if (refreshToken is null || refreshToken.IsRevoked || refreshToken.Expires < DateTimeOffset.UtcNow)
            return Result<LoginResponse>.Fail("Invalid or expired refresh token.");

        AppUser? appUser = await _userManager.FindByIdAsync(refreshToken.AppUserId.ToString());

        if (appUser is null)
            return Result<LoginResponse>.Fail("There is no user with this refresh token");

        try
        {
            await _unitOfWork.BeginTransactionAsync();

            refreshToken.IsRevoked = true;
            refreshToken.RevokedAt = DateTimeOffset.UtcNow;
            refreshToken.RevokedReason = "Refreshed";

            _unitOfWork.RefreshTokens.Update(refreshToken);

            UserClaimsDto userClaimsDto = _mapper.Map<UserClaimsDto>(appUser);
            string newAccessToken = await _tokenGenerator.GenerateJwtToken(userClaimsDto);
            string newPlainRefreshToken = _tokenGenerator.GenerateToken();
            string newRefreshTokenHash = _tokenHasher.HashToken(newPlainRefreshToken);

            RefreshToken newRefreshToken = new RefreshToken
            {
                AppUserId = appUser.Id,
                TokenHash = newRefreshTokenHash
            };

            _unitOfWork.RefreshTokens.Add(newRefreshToken);

            int changes = await _unitOfWork.CommitTransactionAsync(cancellationToken);

            return changes == 0
                ? Result<LoginResponse>.Fail("Something went wrong while saving changes")
                : Result<LoginResponse>.Ok(new LoginResponse(appUser.UserName!, newAccessToken, newPlainRefreshToken), "User has logged in successfully");
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }

    public async Task<Result> LogoutCurrentDeviceAsync(LogoutCurrentDeviceDto logoutCurrentDeviceDto, CancellationToken cancellationToken)
    {
        string tokenHash = _tokenHasher.HashToken(logoutCurrentDeviceDto.Token);

        RefreshToken? refreshToken = await _unitOfWork.RefreshTokens.GetRefreshTokenByToken(tokenHash, cancellationToken);

        if (refreshToken is null || refreshToken.IsRevoked || refreshToken.Expires <= DateTimeOffset.UtcNow)
            return Result.Fail("Invalid refresh token");

        refreshToken.IsRevoked = true;
        refreshToken.RevokedAt = DateTimeOffset.UtcNow;
        refreshToken.RevokedReason = "Logged out current device";

        int changes = await _unitOfWork.SaveChangesAsync(cancellationToken);

        return changes == 0
            ? Result.Fail("Failed to persist logout operation")
            : Result.Ok("User has successfully logged out current device");
    }

    public async Task<Result> LogoutAllDevicesAsync(Guid userId, CancellationToken cancellationToken)
    {
        AppUser? appUser = await _userManager.FindByIdAsync(userId.ToString());

        if (appUser is null)
            return Result.Fail("User not found");

        IEnumerable<RefreshToken> refreshTokens = await _unitOfWork.RefreshTokens.GetRefreshTokensByUserId(userId, cancellationToken);

        foreach (RefreshToken refreshToken in refreshTokens)
        {
            refreshToken.IsRevoked = true;
            refreshToken.RevokedAt = DateTimeOffset.UtcNow;
            refreshToken.RevokedReason = "User logged out from all devices";
        }

        int changes = await _unitOfWork.SaveChangesAsync(cancellationToken);

        return changes == 0
            ? Result.Fail("Failed to persist logout operation")
            : Result.Ok("User has successfully logged out all devices");
    }

    public async Task<Result> ForgotPasswordAsync(ForgotPasswordUserDto forgotPasswordUserDto)
    {
        AppUser? appUser = await _userManager.FindByEmailAsync(forgotPasswordUserDto.Email);

        if (appUser is null)
            return Result.Fail("User not found");

        string resetToken = await _userManager.GeneratePasswordResetTokenAsync(appUser);

        string encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(resetToken));

        return Result.Ok($"https://localhost:7222/api/Users/ResetPassword?resetToken={encodedToken}");
    }

    public async Task<Result> ResetPasswordAsync(ResetPasswordUserDto resetPasswordUserDto, string resetToken)
    {
        AppUser? appUser = await _userManager.FindByEmailAsync(resetPasswordUserDto.Email);

        if (appUser is null)
            return Result.Fail("Invalid email address");

        string decodedResetToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(resetToken));

        IdentityResult result = await _userManager.ResetPasswordAsync(appUser, decodedResetToken, resetPasswordUserDto.NewPassword);

        return result.Succeeded
            ? Result.Ok("Password has been reset successfully")
            : Result.Fail("Failed to reset password",
            result.Errors
                  .GroupBy(x => x.Code)
                  .ToDictionary(g => g.Key,
                                g => g.Select(x => x.Description).ToArray()));
    }
}
