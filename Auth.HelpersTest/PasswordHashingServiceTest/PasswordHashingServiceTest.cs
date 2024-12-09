using Auth.Helpers;

namespace AuthHelpersTest.PasswordHashingServiceTest;

[TestFixture]
public class PasswordHashingServiceTest
{
    private PasswordHashingService _hashingService;

    [SetUp]
    public void Setup()
    {
        _hashingService = new PasswordHashingService();
    }

    [Test]
    public void HashPassword_ShouldReturnDifferentString()
    {
        var password = "Password123!";
        var hash = _hashingService.HashPassword(password);
        Assert.That(hash, Is.Not.EqualTo(password));
    }

    [Test]
    public void DoPasswordsMatch_ShouldReturnFalse_WhenDifferentStringIsHashed()
    {
        var password = "Password123!";
        var hash = _hashingService.HashPassword(password);
        var result = _hashingService.DoPasswordsMatch("Password456!", hash);
        Assert.That(result, Is.False);
    }

    [Test]
    public void DoPasswordsMatchShouldReturnTrue_WhenPasswordsMatch()
    {
        var password = "Password123!";
        var hash = _hashingService.HashPassword(password);
        var result = _hashingService.DoPasswordsMatch(password, hash);
        Assert.That(result, Is.True);
    }

    [Test]
    public void HashPassword_ShouldReturnDifferentStrings_ForSamePasswordHashedTwice()
    {
        var password = "Password123!";
        var hash1 = _hashingService.HashPassword(password);
        var hash2 = _hashingService.HashPassword(password);
        
        Assert.That(hash1, Is.Not.EqualTo(hash2));
    }
}