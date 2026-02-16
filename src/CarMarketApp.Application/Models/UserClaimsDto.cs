namespace CarMarketApp.Application.Models;

public sealed record UserClaimsDto(
    Guid Id,
    string FirstName,
    string Lastname,
    string Email,
    string UserName);
