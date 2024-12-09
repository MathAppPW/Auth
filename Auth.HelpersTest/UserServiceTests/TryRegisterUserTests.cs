using Auth;
using Auth.Helpers;
using Auth.Models;
using Auth.Utils;
using Moq;

namespace AuthHelpersTest.UserServiceTests;

[TestFixture]
public class TryRegisterUserTests : BaseUserServiceTest
{
    [Test]
    public async Task TryRegisterUser_ShouldReturnFailure_WhenEmailIsInvalid()
    {
        DataValidatorMock
            .Setup(m => m.IsMailSyntaxValid(It.IsAny<string>(), out It.Ref<string>.IsAny))
            .Returns(false);
        await TryRegisterUser_ShouldReturnFailure_ForInvalidData();
    }

    [Test]
    public async Task TryRegisterUser_ShouldReturnFailure_WhenPasswordIsInvalid()
    {
        DataValidatorMock
            .Setup(m => m.IsMailSyntaxValid(It.IsAny<string>(), out It.Ref<string>.IsAny))
            .Returns(true);
        DataValidatorMock
            .Setup(m => m.IsPasswordValid(It.IsAny<string>(), out It.Ref<string>.IsAny))
            .Returns(false);
        await TryRegisterUser_ShouldReturnFailure_ForInvalidData();
    }

    [Test]
    public async Task TryRegisterUser_ShouldReturnFailure_WhenUserNotFound_()
    {
        SetDataValidatorToWork();
        UserRepoMock
            .Setup(m => m.GetOneByMail(It.IsAny<string>())).ReturnsAsync(TestUser);
        await TryRegisterUser_ShouldReturnFailure_WhenUserNotFound();
    }

    [Test]
    public async Task TryRegisterUser_ShouldReturnSuccess_WhenWorks()
    {
        SetDataValidatorToWork();
        SetUserRepoToWork();
        SetHasherToWork();
        var result = await GetRegistrationResult();
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Status, Is.EqualTo(RegisterResponse.Types.Status.Success));
        });
    }
    
    private async Task TryRegisterUser_ShouldReturnFailure_ForInvalidData()
    {
        var result = await GetRegistrationResult();
        Assert.Multiple(() =>
        {
            Assert.That(result.IsFailure, Is.True);
            Assert.That(result.Status, Is.EqualTo(RegisterResponse.Types.Status.InvalidData));
        });
    }

    private async Task TryRegisterUser_ShouldReturnFailure_WhenUserNotFound()
    {
        var result = await GetRegistrationResult();
        Assert.Multiple(() =>
        {
            Assert.That(result.IsFailure, Is.True);
            Assert.That(result.Status, Is.EqualTo(RegisterResponse.Types.Status.EmailConflict));
        });
    }

    private async Task<RegistrationResult> GetRegistrationResult()
    {
        var userService = new UserService(UserRepo, DataValidationService, PasswordHashingService, Logger);
        var result = await userService.TryRegisterUser(TestUserDto);
        return result;
    }
    
    private void SetDataValidatorToWork()
    {
        DataValidatorMock
            .Setup(m => m.IsMailSyntaxValid(It.IsAny<string>(), out It.Ref<string>.IsAny))
            .Returns(true);
        DataValidatorMock
            .Setup(m => m.IsPasswordValid(It.IsAny<string>(), out It.Ref<string>.IsAny))
            .Returns(true);
    }

    private void SetUserRepoToWork()
    {
        UserRepoMock
            .Setup(m => m.GetOneByMail(It.IsAny<string>()))
            .ReturnsAsync((User?)null); //the cast is necessary because of ambiguous call otherwise
        UserRepoMock
            .Setup(m => m.AddUser(It.IsAny<User>()))
            .ReturnsAsync((User user) => user);
    }

    private void SetHasherToWork()
    {
        PasswordHashingMock
            .Setup(m => m.HashPassword(It.IsAny<string>()))
            .Returns((string s) => s);
    }
}