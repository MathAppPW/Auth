using Auth.Helpers;

namespace AuthHelpersTest.DataValidationServiceTests;

[TestFixture]
public class PasswordValidationTest
{
    private DataValidationService _dataValidationService;
    
    [SetUp]
    public void Setup()
    {
        _dataValidationService = new DataValidationService();
    }

    [Test]
    public void IsPasswordValid_ShouldReturnFalseForEmptyString()
        => IsPasswordValid_ShouldReturnFalse("");

    [Test]
    public void IsPasswordValid_ShouldReturnFalseForTooShortPassword()
        => IsPasswordValid_ShouldReturnFalse("aB!0");

    [Test]
    public void IsPasswordValid_ShouldReturnFalseForTooLongPassword()
        => IsPasswordValid_ShouldReturnFalse(
            "aB!0daiodjsodjioasjdioandioo1903210NSQsodJJSA*(DASKDSAODJASOJDISOADIOSDNCXCOACOICMIMICSAMIOCAOMICAMIOCDMIOCDMIOCDMIOCDMIOCDMIOCD");

    [Test]
    public void IsPasswordValid_ShouldReturnFalseForPasswordWithoutASpecialCharacter()
        => IsPasswordValid_ShouldReturnFalse("aAm12DmmdaoO");

    [Test]
    public void IsPasswordValid_ShouldReturnFalseForPasswordWithoutLargeLetter()
        => IsPasswordValid_ShouldReturnFalse("!@asd1233dsa");

    [Test]
    public void IsPasswordValid_ShouldReturnFalseForPasswordWithoutSmallLetter()
        => IsPasswordValid_ShouldReturnFalse("ASDA!@DSA123");

    [Test]
    public void IsPasswordValid_ShouldReturnFalseForPasswordWithoutDigit()
        => IsPasswordValid_ShouldReturnFalse("ADA!@#Ddoaidmoasi");

    [Test]
    public void ShouldReturnTrueForAValidPassword()
    {
        var result = _dataValidationService.IsPasswordValid("ThisIsAValidPassword123!!", out _);
        Assert.That(result, Is.True);
    }
    
    private void IsPasswordValid_ShouldReturnFalse(string password)
    {
        var result = _dataValidationService.IsPasswordValid(password, out _);
        Assert.That(result, Is.False);
    }
}