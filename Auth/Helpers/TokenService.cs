using System.Security.Claims;
using System.Text;
using Auth.Interfaces;
using Auth.Models;
using Auth.Utils;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Auth.Helpers;

public class TokenService : ITokenService
{
    private readonly JwtSettings _authSettings;
    private readonly JwtSettings _refreshSettings;

    public TokenService(IOptionsSnapshot<JwtSettings> jwtSettings)
    {
        _authSettings = jwtSettings.Get("Auth");
        _refreshSettings = jwtSettings.Get("Refresh");
        Console.WriteLine("auth: "+_authSettings.SecretKey);
        Console.WriteLine("ref: " + _refreshSettings.SecretKey);
    }

    public string GetLoginToken(User user) => GenerateToken(_authSettings, user);
    public string GetRefreshToken(User user) => GenerateToken(_refreshSettings, user);

    public async Task<string?> VerifyRefreshToken(string token) => await VerifyToken(_refreshSettings, token);//zwraca mail uytkownika
    
    private async Task<string?> VerifyToken(JwtSettings settings, string token)
    {
        var tokenHandler = new JsonWebTokenHandler();

        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = settings.Issuer,
            ValidateAudience = true,
            ValidAudience = settings.Audience,
            ValidateLifetime = true,
            IssuerSigningKey = GenerateSymmetricSecurityKey(settings.SecretKey),
            ValidateIssuerSigningKey = true
        };

        var result = await tokenHandler.ValidateTokenAsync(token, validationParameters);
        
        if (!result.IsValid) return null;
        var jwToken = new JsonWebToken(token);
        var mail = jwToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;
        return mail;

    }
    
    private string GenerateToken(JwtSettings settings, User user)
    {
        var tokenHandler = new JsonWebTokenHandler();
        var tokenDescriptor = GenerateTokenDescriptor(settings, user);
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return token;
    }

    private SecurityTokenDescriptor GenerateTokenDescriptor(JwtSettings settings, User user)
    {
        var signingCredentials = GenerateSigningCredentials(settings);
        var claims = GenerateClaims(settings, user);
        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(settings.TokenLifeTimeInMinutes),
            Issuer = settings.Issuer,
            Audience = settings.Audience,
            SigningCredentials = signingCredentials
        };
        return tokenDescriptor;
    }

    private Claim[] GenerateClaims(JwtSettings settings, User user)
    {
        return
        [
            new Claim(JwtRegisteredClaimNames.Iss, settings.Issuer),
            new Claim(JwtRegisteredClaimNames.Sub, user.Mail),
            new Claim(JwtRegisteredClaimNames.Aud, settings.Audience)
        ];
    }

    private SigningCredentials GenerateSigningCredentials(JwtSettings settings)
    {
        var secretKey = GenerateSymmetricSecurityKey(settings.SecretKey);
        var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
        return signingCredentials;
    }

    private SymmetricSecurityKey GenerateSymmetricSecurityKey(string secretKey)
    {
        return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
    }
}
