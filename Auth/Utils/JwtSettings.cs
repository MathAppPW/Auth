namespace Auth.Utils;

public class JwtSettings
{
    public string SecretKey { get; set; }
    public int TokenLifeTimeInMinutes { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
}