using CarMarketApp.Application.DTOs.Adverts;
using CarMarketApp.Application.Models;
using CarMarketApp.Application.Models.ResultPattern;

namespace CarMarketApp.Application.Abstractions.Services;

public interface IAdvertService
{
    Task<Result> CreateAdvertAsync(CreateAdvertDto createAdvertDto, Guid userId, CancellationToken cancellationToken);
    Task<Result> UpdateAdvertAsync(UpdateAdvertDto updateAdvertDto, CancellationToken cancellationToken);
    Task<Result> DeleteAdvertAsync(Guid advertId, CancellationToken cancellationToken);
    Task<Result> RestoreAdvertAsync(Guid advertId, CancellationToken cancellationToken);
    Task<Result<PagedList<AdvertDto>>> GetAllAdvertsAsync(AdvertFilterDto advertFilterDto, CancellationToken cancellationToken);
}
