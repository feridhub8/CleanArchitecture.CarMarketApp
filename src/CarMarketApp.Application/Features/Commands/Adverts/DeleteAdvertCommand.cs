using CarMarketApp.Application.Models.ResultPattern;
using MediatR;

namespace CarMarketApp.Application.Features.Commands.Adverts;

public sealed record DeleteAdvertCommand(
    Guid AdvertId,
    Guid UserId) : IRequest<Result>;
