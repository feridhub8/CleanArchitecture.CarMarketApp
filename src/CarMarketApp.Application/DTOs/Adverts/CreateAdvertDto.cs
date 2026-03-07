namespace CarMarketApp.Application.DTOs.Adverts;

public sealed record CreateAdvertDto(
    int Price,
    int Year,
    int Mileage,
    string Description,
    Guid ModelId);
