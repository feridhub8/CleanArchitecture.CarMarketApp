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

        using SHA256 sha256 = SHA256.Create();
        byte[] bytes = Encoding.UTF8.GetBytes(token);
        byte[] hashBytes = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hashBytes);
    }

    public bool VerifyToken(string token, string tokenHash)
    {
        if (string.IsNullOrWhiteSpace(token) || string.IsNullOrWhiteSpace(tokenHash))
            return false;

        string hashedInput = HashToken(token);
        return string.Equals(hashedInput, tokenHash);
    }
}
