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

        byte[] computedHash;

        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] tokenBytes = Encoding.UTF8.GetBytes(token);
            computedHash = sha256.ComputeHash(tokenBytes);
        }

        byte[] storedHash;

        try
        {
            storedHash = Convert.FromBase64String(tokenHash);
        }
        catch
        {
            return false;
        }

        return CryptographicOperations.FixedTimeEquals(computedHash, storedHash);
    }
}
