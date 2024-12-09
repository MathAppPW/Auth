using Auth.Helpers.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Auth.Helpers;

public class PasswordHashingService : IPasswordHashingService
{
    public string HashPassword(string password)
    {
        var passwordHasher = new PasswordHasher<object>();
        var hash = passwordHasher.HashPassword(null, password);
        return hash;
    }

    public bool DoPasswordsMatch(string password, string hash)
    {
        var passwordVerifier = new PasswordHasher<object>();
        var result = passwordVerifier.VerifyHashedPassword(null, hash, password);
        return result == PasswordVerificationResult.Success;
    }
}