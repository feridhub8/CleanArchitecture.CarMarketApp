namespace CarMarketApp.Application.DTOs.Adverts;

public sealed record UpdateAdvertDto(
    Guid Id,
    int Price,
    int Year,
    int Mileage,
    string Description,
    Guid ModelId);
