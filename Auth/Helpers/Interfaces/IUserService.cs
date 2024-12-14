using Auth.Models;
using Auth.Utils;

namespace Auth.Helpers.Interfaces;

public interface IUserService
{
    Task<RegistrationResult> TryRegisterUser(UserDto dto);
    Task<ValidationResult> ValidateUser(UserDto dto);
    Task<User?> GetUser(string mail);
}