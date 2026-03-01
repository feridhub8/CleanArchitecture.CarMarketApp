using CarMarketApp.Domain.Entities;

namespace CarMarketApp.Application.Abstractions.Repositories;

public interface IModelRepository
{
    void Add(Model model);
    void Update(Model model);
    Task<Model?> GetModelByNameIgnoringQueryFilterAsync(string normalizedName, Guid brandId, CancellationToken cancellationToken);
    Task<Model?> GetModelByIdIgnoringQueryFilterAsync(Guid modelId, CancellationToken cancellationToken);
    IQueryable<Model> GetAllModelsQuery();
}
