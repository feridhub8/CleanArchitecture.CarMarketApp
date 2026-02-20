using CarMarketApp.Application.DTOs.Identity;
using CarMarketApp.Application.Models.ResultPattern;
using MediatR;

namespace CarMarketApp.Application.Features.Commands.Identity;

public sealed record ForgotPasswordUserCommand(ForgotPasswordUserDto ForgotPasswordUserDto) : IRequest<Result>;
