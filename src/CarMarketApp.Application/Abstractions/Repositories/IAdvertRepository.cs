using CarMarketApp.Domain.Entities;

namespace CarMarketApp.Application.Abstractions.Repositories;

public interface IAdvertRepository
{
    void Add(Advert advert);
    void Update(Advert advert);
    Task<Advert?> GetAdvertByIdIgnoringQueryFilterAsync(Guid advertId, CancellationToken cancellationToken);
    IQueryable<Advert> GetAllAdvertsQuery();
}
