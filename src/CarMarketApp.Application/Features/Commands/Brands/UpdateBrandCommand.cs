using CarMarketApp.Application.DTOs.Brands;
using CarMarketApp.Application.Models.ResultPattern;
using MediatR;

namespace CarMarketApp.Application.Features.Commands.Brands;

public sealed record UpdateBrandCommand(UpdateBrandDto UpdateBrandDto) : IRequest<Result>;
