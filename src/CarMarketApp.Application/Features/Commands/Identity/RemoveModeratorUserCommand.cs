using CarMarketApp.Application.Models.ResultPattern;
using MediatR;

namespace CarMarketApp.Application.Features.Commands.Identity;

public sealed record RemoveModeratorUserCommand(Guid UserId) : IRequest<Result>;
