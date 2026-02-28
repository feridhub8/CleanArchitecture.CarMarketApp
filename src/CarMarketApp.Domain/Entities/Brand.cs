using CarMarketApp.Domain.Common;

namespace CarMarketApp.Domain.Entities;

public sealed class Brand : BaseEntity
{
    public string Name { get; set; } = default!;
    public string NormalizedName { get; set; } = default!;

    public string Normalize(string name)
    {
        return name.Trim().ToUpperInvariant();
    }
}
