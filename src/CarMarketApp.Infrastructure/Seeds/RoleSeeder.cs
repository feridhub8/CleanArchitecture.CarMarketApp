using CarMarketApp.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;

namespace CarMarketApp.Infrastructure.Seeds;

public static class RoleSeeder
{
    public static async Task SeedRolesAsync(RoleManager<AppRole> roleManager)
    {
        string[] roles = ["User", "Moderator", "Admin"];

        foreach (var roleName in roles)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new AppRole { Name = roleName });
            }
        }
    }
}
