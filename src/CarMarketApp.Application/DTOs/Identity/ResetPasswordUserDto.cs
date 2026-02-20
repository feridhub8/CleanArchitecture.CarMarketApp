namespace CarMarketApp.Application.DTOs.Identity;

public sealed record ResetPasswordUserDto(string Email, string NewPassword);
