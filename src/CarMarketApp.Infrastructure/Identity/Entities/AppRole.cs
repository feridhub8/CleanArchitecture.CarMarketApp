using Microsoft.AspNetCore.Identity;

namespace CarMarketApp.Infrastructure.Identity.Entities;

public sealed class AppRole : IdentityRole<Guid>
{
    public ICollection<AppUserRole> UserRoles { get; set; } = new List<AppUserRole>();
}
