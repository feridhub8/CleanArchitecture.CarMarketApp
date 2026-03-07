using CarMarketApp.Application.Models.ResultPattern;
using MediatR;

namespace CarMarketApp.Application.Features.Commands.Adverts;

public sealed record RestoreAdvertCommand(Guid AdvertId) : IRequest<Result>;
