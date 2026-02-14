namespace CarMarketApp.Application.DTOs.Users;

public sealed record RegisterUserDto(
    string FirstName,
    string LastName,
    string Email,
    string UserName,
    string Password);
