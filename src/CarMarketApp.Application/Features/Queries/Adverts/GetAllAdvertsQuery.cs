using CarMarketApp.Application.DTOs.Adverts;
using CarMarketApp.Application.Models;
using CarMarketApp.Application.Models.ResultPattern;
using MediatR;

namespace CarMarketApp.Application.Features.Queries.Adverts;

public sealed record GetAllAdvertsQuery(AdvertFilterDto AdvertFilterDto) : IRequest<Result<PagedList<AdvertDto>>>;
