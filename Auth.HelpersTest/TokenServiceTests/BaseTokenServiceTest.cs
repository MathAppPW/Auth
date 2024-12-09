using Auth.Helpers;
using Auth.Utils;
using Microsoft.Extensions.Options;
using Moq;

namespace AuthHelpersTest.TokenServiceTests;

public class BaseTokenServiceTest
{
    protected TokenService TokenService { get; private set;  }

    [SetUp]
    public void SetUp()
    {
        var authSettings = new JwtSettings()
        {
            SecretKey = "SuperSecretKey",
            TokenLifeTimeInMinutes = 30,
            Issuer = "MathApp.Auth",
            Audience = "MathApp"
        };
        var refreshSettings = new JwtSettings()
        {
            SecretKey = "SuperSecretKey",
            TokenLifeTimeInMinutes = 24 * 60 * 30,
            Issuer = "MathApp.Auth",
            Audience = "MathApp"
        };

        var optionsMock = new Mock<IOptionsSnapshot<JwtSettings>>();
        optionsMock.Setup(m => m.Get("Auth")).Returns(authSettings);
        optionsMock.Setup(m => m.Get("Refresh")).Returns(refreshSettings);
        TokenService = new TokenService(optionsMock.Object);
    }
}
