namespace CarMarketApp.Application.DTOs.Brands;

public sealed record BrandFilterDto
{
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    public string? Search { get; init; }
}
