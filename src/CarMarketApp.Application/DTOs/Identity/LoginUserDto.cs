namespace CarMarketApp.Application.DTOs.Users;

public sealed record LoginUserDto(
    string EmailOrUserName,
    string Password);
