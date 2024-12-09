using Auth.Helpers;
using Auth.Models;
using Auth.Utils;
using Moq;

namespace AuthHelpersTest.UserServiceTests;

[TestFixture]
public class ValidateUserTest : BaseUserServiceTest
{
    [Test]
    public async Task ValidateUser_ShouldFail_WhenUserDoesNotExist()
    {
        UserRepoMock.Setup(m => m.GetOneByMail(It.IsAny<string>())).ReturnsAsync((User?)null);
        await ValidateUser_ShouldFail();
    }

    [Test]
    public async Task ValidateUser_ShouldFail_WhenPasswordsDoNotMatch()
    {
        SetUserRepoToWork();
        PasswordHashingMock.Setup(m => m.DoPasswordsMatch(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(false);
        await ValidateUser_ShouldFail();
    }

    [Test]
    public async Task ValidateUser_ShouldSucceed_WhenEverythingIsAlright()
    {
        SetUserRepoToWork();
        SetPasswordHasherToWork();
        await ValidateUser_ShouldSucceed();
    }
    
    private void SetUserRepoToWork()
    {
        UserRepoMock.Setup(m => m.GetOneByMail(It.IsAny<string>()))
            .ReturnsAsync(TestUser);
    }

    private void SetPasswordHasherToWork()
    {
        PasswordHashingMock.Setup(m => m.DoPasswordsMatch(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(true);
    }
    
    private async Task ValidateUser_ShouldFail()
    {
        var result = await GetValidationResult();
        Assert.That(result.IsFailure, Is.True);
    }

    private async Task ValidateUser_ShouldSucceed()
    {
        var result = await GetValidationResult();
        Assert.That(result.IsSuccess, Is.True);
    }
    
    private async Task<ValidationResult> GetValidationResult()
    {
        var userService = new UserService(UserRepo, DataValidationService, PasswordHashingService, Logger);
        var result = await userService.ValidateUser(TestUserDto);
        return result;
    }
}