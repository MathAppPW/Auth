namespace Auth.Helpers.Interfaces;

public interface IPasswordHashingService //IPasswordHasher is in the standard library
{
    string HashPassword(string password);
    bool DoPasswordsMatch(string password, string hash);
}