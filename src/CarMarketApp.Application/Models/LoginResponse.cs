namespace CarMarketApp.Application.Models;

public sealed record LoginResponse(
    string UserName,
    string JwtToken,
    string RefreshToken);
