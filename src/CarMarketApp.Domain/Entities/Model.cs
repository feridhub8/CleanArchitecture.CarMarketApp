using CarMarketApp.Domain.Common;

namespace CarMarketApp.Domain.Entities;

public sealed class Model : BaseEntity
{
    public string Name { get; set; } = default!;
    public string NormalizedName { get; set; } = default!;

    public Guid BrandId { get; set; }
    public Brand? Brand { get; set; }

    public string Normalize(string name)
    {
        return name.Trim().ToUpperInvariant();
    }
}
