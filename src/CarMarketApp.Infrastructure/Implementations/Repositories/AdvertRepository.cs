using CarMarketApp.Application.Abstractions.Repositories;
using CarMarketApp.Domain.Entities;
using CarMarketApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CarMarketApp.Infrastructure.Implementations.Repositories;

public sealed class AdvertRepository : IAdvertRepository
{
    private readonly ApplicationDbContext _context;

    public AdvertRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public void Add(Advert advert)
    {
        _context.Add(advert);
    }

    public void Update(Advert advert)
    {
        _context.Update(advert);
    }

    public async Task<Advert?> GetAdvertByIdIgnoringQueryFilterAsync(Guid advertId, CancellationToken cancellationToken)
    {
        return await _context.Adverts.IgnoreQueryFilters().FirstOrDefaultAsync(a => a.Id == advertId, cancellationToken);
    }

    public IQueryable<Advert> GetAllAdvertsQuery()
    {
        return _context.Adverts.AsQueryable();
    }
}
