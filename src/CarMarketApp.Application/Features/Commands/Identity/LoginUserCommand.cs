using CarMarketApp.Application.DTOs.Users;
using CarMarketApp.Application.Models;
using CarMarketApp.Application.Models.ResultPattern;
using MediatR;

namespace CarMarketApp.Application.Features.Commands.Users;

public sealed record LoginUserCommand(LoginUserDto LoginUserDto) : IRequest<Result<LoginResponse>>;
