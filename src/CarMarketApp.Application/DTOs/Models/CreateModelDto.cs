namespace CarMarketApp.Application.DTOs.Models;

public sealed record CreateModelDto(
    string Name,
    Guid BrandId);
