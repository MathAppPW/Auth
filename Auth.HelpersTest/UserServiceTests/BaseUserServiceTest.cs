using Auth.Dal.Interfaces;
using Auth.Helpers;
using Auth.Helpers.Interfaces;
using Auth.Interfaces;
using Auth.Models;
using Microsoft.Extensions.Logging;
using Moq;

namespace AuthHelpersTest.UserServiceTests;

public abstract class BaseUserServiceTest
{
    protected Mock<IUserRepo> UserRepoMock { get; private set; }
    protected Mock<IDataValidationService> DataValidatorMock { get; private set; }
    protected Mock<IPasswordHashingService> PasswordHashingMock { get; private set; }
    protected Mock<ITokenService> TokenServiceMock { get; private set; }
    protected Mock<ILogger<UserService>> LoggerMock { get; private set; }
    
    
    protected IUserRepo UserRepo => UserRepoMock.Object;
    protected IDataValidationService DataValidationService => DataValidatorMock.Object;
    protected IPasswordHashingService PasswordHashingService => PasswordHashingMock.Object;
    protected ITokenService TokenService => TokenServiceMock.Object;
    protected ILogger<UserService> Logger => LoggerMock.Object;
    protected UserDto TestUserDto => new() { Mail = "valid@mail.com", Password = "ValidPassword123!" };
    protected User TestUser => new() { Id = "userid", Mail = "valid@mail.com", PasswordHash = "ValidPassword123!" };
    
    [SetUp]
    public virtual void Setup()
    {
        UserRepoMock = new();
        DataValidatorMock = new();
        PasswordHashingMock = new();
        TokenServiceMock = new();
        LoggerMock = new();
    }
}