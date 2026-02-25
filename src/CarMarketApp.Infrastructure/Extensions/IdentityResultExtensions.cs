using Microsoft.AspNetCore.Identity;

namespace CarMarketApp.Infrastructure.Extensions;

public static class IdentityResultExtensions
{
    public static Dictionary<string, string[]> GetErrors(
        this IdentityResult identityResult)
    {
        return identityResult.Errors
            .GroupBy(x => x.Code)
            .ToDictionary(
                g => g.Key,
                g => g.Select(x => x.Description).ToArray());
    }
}
