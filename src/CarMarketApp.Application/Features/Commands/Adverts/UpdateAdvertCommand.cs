using CarMarketApp.Application.DTOs.Adverts;
using CarMarketApp.Application.Models.ResultPattern;
using MediatR;

namespace CarMarketApp.Application.Features.Commands.Adverts;

public sealed record UpdateAdvertCommand(UpdateAdvertDto UpdateAdvertDto) : IRequest<Result>;
