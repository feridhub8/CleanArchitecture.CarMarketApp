using CarMarketApp.Application.Abstractions.Helpers;
using CarMarketApp.Application.Models;
using CarMarketApp.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace CarMarketApp.Infrastructure.Implementations.Helpers;

public sealed class TokenGenerator : ITokenGenerator
{
    private readonly JwtSettings _jwtSettings;
    private readonly UserManager<AppUser> _userManager;

    public TokenGenerator(IOptions<JwtSettings> jwtSettins, UserManager<AppUser> userManager)
    {
        _jwtSettings = jwtSettins.Value;
        _userManager = userManager;
    }

    public string GenerateToken()
    {
        var bytes = RandomNumberGenerator.GetBytes(64);
        return Convert.ToBase64String(bytes)
                      .TrimEnd('=')
                      .Replace('+', '-')
                      .Replace('/', '_');
    }

    public async Task<string> GenerateJwtToken(UserClaimsDto userClaimsDto)
    {
        var secretKey = _jwtSettings.SecretKey;
        var issuer = _jwtSettings.Issuer;
        var audience = _jwtSettings.Audience;
        var expiryMinutes = _jwtSettings.ExpiryMinutes;

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userClaimsDto.Id.ToString()),
            new Claim(ClaimTypes.Name, userClaimsDto.FirstName),
            new Claim(ClaimTypes.Surname, userClaimsDto.Lastname),
            new Claim(ClaimTypes.Email, userClaimsDto.Email),
            new Claim("username", userClaimsDto.UserName)
        };

        AppUser? user = await _userManager.FindByIdAsync(userClaimsDto.Id.ToString());
        if (user is null)
            throw new Exception("User not found");

        var roles = await _userManager.GetRolesAsync(user);

        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
