using CarMarketApp.Application.Models.ResultPattern;
using MediatR;

namespace CarMarketApp.Application.Features.Commands.Models;

public sealed record DeleteModelCommand(Guid ModelId) : IRequest<Result>;
