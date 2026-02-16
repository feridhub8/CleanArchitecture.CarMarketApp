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
using Microsoft.EntityFrameworkCore;

namespace CarMarketApp.Infrastructure.Implementations.Services;

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

        if (!identityResult.Succeeded)
            return Result.Fail("Something went wrong while adding role to user");

        return Result.Ok("User has successfully registered");
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

        int saveResult = await _unitOfWork.SaveChangesAsync(cancellationToken);

        if (saveResult < 1)
            return Result<LoginResponse>.Fail("Something went wrong while saving changes");

        LoginResponse loginResponse = new LoginResponse(user.UserName!, accessToken, plainRefreshToken);

        return Result<LoginResponse>.Ok(loginResponse, "User has logged in successfully");
    }

    public async Task<Result<LoginResponse>> RefreshTokenAsync(RefreshTokenDto refreshTokenDto, CancellationToken cancellationToken)
    {
        string tokenHash = _tokenHasher.HashToken(refreshTokenDto.Token);

        RefreshToken? refreshToken = await _unitOfWork.RefreshTokens.GetRefreshTokenByToken(tokenHash, cancellationToken);

        if (refreshToken is null || refreshToken.IsRevoked || refreshToken.Expires < DateTime.UtcNow)
            return Result<LoginResponse>.Fail("Invalid or expired refresh token.");

        AppUser? appUser = await _userManager.FindByIdAsync(refreshToken.AppUserId.ToString());

        if (appUser is null)
            return Result<LoginResponse>.Fail("There is no user with this refresh token");

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

        int saveResult = await _unitOfWork.CommitTransactionAsync(cancellationToken);

        if (saveResult < 1)
            return Result<LoginResponse>.Fail("Something went wrong while saving changes");

        LoginResponse loginResponse = new LoginResponse(appUser.UserName!, newAccessToken, newPlainRefreshToken);

        return Result<LoginResponse>.Ok(loginResponse, "User has logged in successfully");

    }
}
