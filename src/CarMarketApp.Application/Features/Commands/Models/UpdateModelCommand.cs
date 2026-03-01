using CarMarketApp.Application.DTOs.Models;
using CarMarketApp.Application.Models.ResultPattern;
using MediatR;

namespace CarMarketApp.Application.Features.Commands.Models;

public sealed record UpdateModelCommand(UpdateModelDto UpdateModelDto) : IRequest<Result>;
