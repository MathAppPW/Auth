using Auth.Models;

namespace Auth.Interfaces;

public interface ITokenService
{
    public string GetLoginToken(User user);
    public string GetRefreshToken(User user);
    public Task<string?> VerifyRefreshToken(string token);
}