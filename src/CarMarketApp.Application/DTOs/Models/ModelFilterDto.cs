namespace CarMarketApp.Application.DTOs.Models;

public sealed class ModelFilterDto
{
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    public string? Search { get; init; }
}
