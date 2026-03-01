using CarMarketApp.Application.Abstractions.Repositories;
using CarMarketApp.Application.Models.ResultPattern;
using CarMarketApp.Domain.Entities;
using CarMarketApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CarMarketApp.Infrastructure.Implementations.Repositories;

public sealed class ModelRepository : IModelRepository
{
    private readonly ApplicationDbContext _context;

    public ModelRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public void Add(Model model)
    {
        _context.Add(model);
    }

    public void Update(Model model)
    {
        _context.Update(model);
    }

    public async Task<Model?> GetModelByNameIgnoringQueryFilterAsync(string normalizedName, Guid brandId, CancellationToken cancellationToken)
    {
        return await _context.Models.IgnoreQueryFilters().FirstOrDefaultAsync(m => m.NormalizedName == normalizedName && m.BrandId == brandId, cancellationToken);
    }

    public async Task<Model?> GetModelByIdIgnoringQueryFilterAsync(Guid modelId, CancellationToken cancellationToken)
    {
        return await _context.Models.IgnoreQueryFilters().FirstOrDefaultAsync(m => m.Id == modelId, cancellationToken);
    }

    public IQueryable<Model> GetAllModelsQuery()
    {
        return _context.Models.AsQueryable();
    }
}
