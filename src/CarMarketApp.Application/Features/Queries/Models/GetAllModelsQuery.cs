using CarMarketApp.Application.DTOs.Models;
using CarMarketApp.Application.Models;
using CarMarketApp.Application.Models.ResultPattern;
using MediatR;

namespace CarMarketApp.Application.Features.Queries.Models;

public sealed record GetAllModelsQuery(ModelFilterDto ModelFilterDto) : IRequest<Result<PagedList<ModelDto>>>;
