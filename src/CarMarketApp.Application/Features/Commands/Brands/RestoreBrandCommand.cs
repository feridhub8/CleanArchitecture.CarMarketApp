using CarMarketApp.Application.Models.ResultPattern;
using MediatR;

namespace CarMarketApp.Application.Features.Commands.Brands;

public sealed record RestoreBrandCommand(Guid Id) : IRequest<Result>;
