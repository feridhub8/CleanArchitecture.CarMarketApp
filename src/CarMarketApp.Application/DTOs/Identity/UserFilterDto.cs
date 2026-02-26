namespace CarMarketApp.Application.DTOs.Identity;

public sealed record UserFilterDto
{
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    public string? Search { get; init; }
    public string? Role { get; init; }
}
