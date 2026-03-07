using CarMarketApp.Application.DTOs.Adverts;
using CarMarketApp.Application.Models.ResultPattern;
using MediatR;

namespace CarMarketApp.Application.Features.Commands.Adverts;

public sealed record CreateAdvertCommand(
    CreateAdvertDto CreateAdvertDto,
    Guid UserId) : IRequest<Result>;
