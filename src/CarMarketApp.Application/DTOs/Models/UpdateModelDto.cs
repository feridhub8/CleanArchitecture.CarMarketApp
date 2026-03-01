namespace CarMarketApp.Application.DTOs.Models;

public sealed record UpdateModelDto(
    Guid Id,
    string Name,
    Guid BrandId);
