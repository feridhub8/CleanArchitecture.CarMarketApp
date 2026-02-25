namespace CarMarketApp.Application.DTOs.Identity;

public sealed record UserDto(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    string UserName,
    bool IsDeleted);
