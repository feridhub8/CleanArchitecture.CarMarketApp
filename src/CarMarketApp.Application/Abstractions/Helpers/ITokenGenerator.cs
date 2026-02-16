using CarMarketApp.Application.Models;

namespace CarMarketApp.Application.Abstractions.Helpers;

public interface ITokenGenerator
{
    Task<string> GenerateJwtToken(UserClaimsDto userClaimsDto);
    string GenerateToken();
}
