using CarMarketApp.Application.Abstractions.Helpers;
using System.Security.Cryptography;
using System.Text;

namespace CarMarketApp.Infrastructure.Implementations.Helpers;

public class TokenHasher : ITokenHasher
{
    public string HashToken(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
            throw new ArgumentException("Token cannot be null or empty", nameof(token));

        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(token);
        var hashBytes = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hashBytes);
    }

    public bool VerifyToken(string token, string tokenHash)
    {
        if (string.IsNullOrWhiteSpace(token) || string.IsNullOrWhiteSpace(tokenHash))
            return false;

        var hashedInput = HashToken(token);
        return hashedInput == tokenHash;
    }
}
