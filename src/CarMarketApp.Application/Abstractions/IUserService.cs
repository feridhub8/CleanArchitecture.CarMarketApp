using CarMarketApp.Application.DTOs.Users;
using CarMarketApp.Application.Models.ResultPattern;

namespace CarMarketApp.Application.Abstractions;

public interface IUserService
{
    Task<Result> RegisterUserAsync(RegisterUserDto registerUserDto);
}
