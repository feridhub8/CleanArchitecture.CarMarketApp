using CarMarketApp.Application.DTOs.Brands;
using CarMarketApp.Application.Models;
using CarMarketApp.Application.Models.ResultPattern;
using MediatR;

namespace CarMarketApp.Application.Features.Queries.Brands;

public sealed record GetAllBrandsQuery(BrandFilterDto BrandFilterDto) : IRequest<Result<PagedList<BrandDto>>>;
