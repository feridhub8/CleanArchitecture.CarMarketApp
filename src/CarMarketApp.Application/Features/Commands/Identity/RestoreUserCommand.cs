using CarMarketApp.Application.Models.ResultPattern;
using MediatR;

namespace CarMarketApp.Application.Features.Commands.Identity;

public sealed record RestoreUserCommand(Guid UserId) : IRequest<Result>;
