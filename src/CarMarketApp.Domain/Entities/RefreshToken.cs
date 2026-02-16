namespace CarMarketApp.Domain.Entities;

public sealed class RefreshToken
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string TokenHash { get; set; } = default!;
    public DateTimeOffset Expires { get; set; } = DateTimeOffset.UtcNow.AddDays(7);
    public bool IsRevoked { get; set; } = false;
    public DateTimeOffset? RevokedAt { get; set; }
    public string? RevokedReason { get; set; }

    public Guid AppUserId { get; set; }
}
