using Microsoft.AspNetCore.Identity;

namespace CarMarketApp.Infrastructure.Identity.Entities;

public sealed class AppUser : IdentityUser<Guid>
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string FullName => string.Join(" ", FirstName, LastName);

    public DateTimeOffset CreatedAt { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }

    public AppUser()
    {
        CreatedAt = DateTimeOffset.UtcNow;
        IsDeleted = false;
    }
}
