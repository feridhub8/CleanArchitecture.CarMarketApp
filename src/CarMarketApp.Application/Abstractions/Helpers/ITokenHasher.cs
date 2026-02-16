namespace CarMarketApp.Application.Abstractions.Helpers;

public interface ITokenHasher
{
    string HashToken(string token);
    bool VerifyToken(string token, string tokenHash);
}
