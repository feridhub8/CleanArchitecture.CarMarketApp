using CarMarketApp.Application.DTOs.Identity;
using CarMarketApp.Application.Models;
using CarMarketApp.Application.Models.ResultPattern;
using MediatR;

namespace CarMarketApp.Application.Features.Queries.Identity;

public sealed record GetAllUsersQuery(int Page, int PageSize) : IRequest<Result<PagedList<UserDto>>>;
