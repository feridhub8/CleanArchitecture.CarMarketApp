using FluentValidation.Results;

namespace CarMarketApp.Application.Extensions;

public static class ValidationResultExtensions
{
    public static Dictionary<string, string[]> GetErrors(this ValidationResult validationResult)
    {
        if (validationResult == null)
            throw new ArgumentNullException(nameof(validationResult));

        return validationResult.Errors
                               .GroupBy(e => e.PropertyName)
                               .ToDictionary(
                                   g => g.Key.Split('.').Last(),
                                   g => g.Select(x => x.ErrorMessage).ToArray()
                               );
    }
}
