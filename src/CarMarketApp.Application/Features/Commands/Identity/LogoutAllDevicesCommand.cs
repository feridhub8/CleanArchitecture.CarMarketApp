using CarMarketApp.Application.Models.ResultPattern;
using MediatR;

namespace CarMarketApp.Application.Features.Commands.Identity;

public sealed record LogoutAllDevicesCommand(Guid UserId) : IRequest<Result>;
