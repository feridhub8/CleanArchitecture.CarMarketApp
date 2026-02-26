using Microsoft.AspNetCore.Identity;

namespace CarMarketApp.Infrastructure.Identity.Entities;

public class AppUserRole : IdentityUserRole<Guid>
{
    public AppUser? User { get; set; }
    public AppRole? Role { get; set; }
}
