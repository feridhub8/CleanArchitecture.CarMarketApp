namespace CarMarketApp.Application.DTOs.Adverts;

public sealed class AdvertFilterDto
{
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    public Guid? BrandId { get; set; }
    public Guid? ModelId { get; set; }
}
