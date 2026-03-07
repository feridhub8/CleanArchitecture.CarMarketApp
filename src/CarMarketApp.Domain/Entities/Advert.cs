using CarMarketApp.Domain.Common;

namespace CarMarketApp.Domain.Entities;

public sealed class Advert : BaseEntity
{
    public int Price { get; set; }
    public int Year { get; set; }
    public int Mileage { get; set; }
    public string Description { get; set; } = default!;

    public Guid UserId { get; set; }
    public Guid ModelId { get; set; }

    public Model Model { get; set; } = default!;
}
