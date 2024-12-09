using Auth.Helpers;

namespace AuthHelpersTest.DataValidationServiceTests;

[TestFixture]
public class MailValidationTests
{
    private DataValidationService _dataValidationService;

    [SetUp]
    public void Setup()
    {
        _dataValidationService = new DataValidationService();
    }

    [Test]
    public void IsMailSyntaxValid_ShouldReturnFalseWhenMailIsEmpty()
        => IsMailSyntaxValid_ShouldReturnFalse("");

    [Test]
    public void IsMailSyntaxValid_ShouldReturnFalseWhenMailIsTooShort()
        => IsMailSyntaxValid_ShouldReturnFalse("a@b.n");

    [Test]
    public void IsMailSyntaxValid_ShouldReturnFalseWhenMailIsInvalid() 
        => IsMailSyntaxValid_ShouldReturnFalse("invalid mail");

    //this and test under that, tie to default .net mail validator behavior
    [Test]
    public void IsMailSyntaxValid_ShouldReturnFalseWhenMailHasSomethingBefore()
        => IsMailSyntaxValid_ShouldReturnFalse("user mail@test.com");

    [Test]
    public void IsMailSyntaxValid_ShouldReturnFalseWhenMailHasSomethingAfter()
        => IsMailSyntaxValid_ShouldReturnFalse("mail@test.com user");

    [Test]
    public void IsMailSyntaxValid_ShouldReturnFalseWhenMailIsTooLong()
        => IsMailSyntaxValid_ShouldReturnFalse("averylongemailaddresswithmultiplecharactersandvariouselementsaddedtoitforshowcasingthelengthlimitforthelocalpartoftheemailaddresswhichisallowedtobeupontosixtyfourcharactersperstandard@averylongdomainnamethatisalsomeanttoshowcasethelengthallowedperstandarduptoonetwentyeightcharacters.com");

    [Test]
    public void IsMailSyntaxValid_ShouldReturnTrueWhenMailIsValid()
    {
        var result = _dataValidationService.IsMailSyntaxValid("valid@mail.com", out _);
        Assert.That(result, Is.True);
    }
    
    private void IsMailSyntaxValid_ShouldReturnFalse(string mail)
    {
        var result = _dataValidationService.IsMailSyntaxValid(mail, out _);
        Assert.That(result, Is.False);
    }
}