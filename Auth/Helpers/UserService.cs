using Auth.Dal.Interfaces;
using Auth.Helpers.Interfaces;
using Auth.Models;
using Auth.Utils;

namespace Auth.Helpers;

public class UserService : IUserService
{
    private readonly IUserRepo _userRepo;
    private readonly IDataValidationService _dataValidator;
    private readonly IPasswordHashingService _hashingService;
    private readonly ILogger<UserService> _logger;

    public UserService(IUserRepo userRepo, IDataValidationService dataValidator, IPasswordHashingService hashingService,
        ILogger<UserService> logger)
    {
        _userRepo = userRepo;
        _dataValidator = dataValidator;
        _hashingService = hashingService;
        _logger = logger;
    }
    
    public async Task<RegistrationResult> TryRegisterUser(UserDto dto)
    {
        if (!_dataValidator.IsMailSyntaxValid(dto.Mail, out var mailMessage))
        {
            LogFailedToRegister(dto, mailMessage);
            return RegistrationResult.Failure(mailMessage, RegisterResponse.Types.Status.InvalidData);
        }

        if (!_dataValidator.IsPasswordValid(dto.Password, out var passwordMessage))
        {
            LogFailedToRegister(dto, passwordMessage);
            return RegistrationResult.Failure(passwordMessage, RegisterResponse.Types.Status.InvalidData);
        }

        var userWMail = await _userRepo.GetOneByMail(dto.Mail);
        if (userWMail != null)
        {
            LogFailedToRegister(dto, $"Mail {dto.Mail} already exists.");
            return RegistrationResult.Failure($"User with mail {dto.Mail} already exists.",
                RegisterResponse.Types.Status.EmailConflict);
        }

        var newUser = new User
        {
            Id = Guid.NewGuid().ToString("N"),
            Mail = dto.Mail,
            PasswordHash = _hashingService.HashPassword(dto.Password)
        };

        var resUser = await _userRepo.AddUser(newUser);
        return RegistrationResult.Success("User has been registered", resUser);
    }

    public async Task<ValidationResult> ValidateUser(UserDto dto)
    {
        var model = await _userRepo.GetOneByMail(dto.Mail);
        if (model == null)
        {
            var message = $"User with mail {dto.Mail} does not exist.";
            LogFailedToLogin(dto, message);
            return ValidationResult.Failure(message);
        }

        if (!_hashingService.DoPasswordsMatch(dto.Password, model.PasswordHash))
        {
            var message = "Invalid password";
            LogFailedToLogin(dto, message);
            return ValidationResult.Failure(message);
        }

        return ValidationResult.Success("Logged in successfully", model);
    }

    public async Task<User?> GetUser(string mail)
    {
        var user = await _userRepo.GetOneByMail(mail);
        return user;
    }
    
    private void LogFailedToRegister(UserDto dto, string message)
    {
        _logger.LogWarning("Failed to register user with mail {mail}. {message}", dto.Mail, message);
    }

    private void LogFailedToLogin(UserDto dto, string message)
    {
        _logger.LogInformation("Failed to log in user with mail {mail}. {message}", dto.Mail, message);
    }
}