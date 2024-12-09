using Auth.Models;

namespace Auth.Utils;

public class RegistrationResult
{
    public static RegistrationResult Success(string message, User user) =>
        new(true, message, RegisterResponse.Types.Status.Success, user);
    
    public static RegistrationResult Failure(string message, RegisterResponse.Types.Status status) =>
        new(false, message, status, null);

    private readonly User? _user;
    public User User => _user ?? throw new InvalidOperationException("Operations failed, you can't access user");
    public string Message { get; }
    public RegisterResponse.Types.Status Status { get; }
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;

    private RegistrationResult(bool isSuccess, string message, RegisterResponse.Types.Status status, User? user)
    {
        IsSuccess = isSuccess;
        Message = message;
        Status = status;
        _user = user;
    }
}