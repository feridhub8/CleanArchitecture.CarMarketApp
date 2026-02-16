using CarMarketApp.Application.DTOs.Users;
using CarMarketApp.Application.Models.ResultPattern;
using MediatR;

namespace CarMarketApp.Application.Features.Commands.Users;

public sealed record RegisterUserCommand(RegisterUserDto RegisterUserDto) : IRequest<Result>;
