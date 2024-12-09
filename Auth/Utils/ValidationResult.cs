using Auth.Models;

namespace Auth.Utils;

public class ValidationResult
{
    public static ValidationResult Success(string message, User user) => new(true, message, user);
    public static ValidationResult Failure(string message) => new(false, message, null);
    
    private readonly User? _user;
    public User User => _user ?? throw new InvalidOperationException("Operation failed, you can't access user");
    public string Message { get; }
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;

    private ValidationResult(bool isSuccess, string message, User? user)
    {
        IsSuccess = isSuccess;
        Message = message;
        _user = user;
    }
}