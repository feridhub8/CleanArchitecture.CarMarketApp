namespace CarMarketApp.Application.DTOs.Adverts;

public sealed record AdvertDto
{
    public int Price { get; set; }
    public int Year { get; set; }
    public int Mileage { get; set; }
    public string Description { get; set; } = default!;
    public string BrandName { get; set; } = default!;
    public string ModelName { get; set; } = default!;
}
