using Auth.Dal.Interfaces;
using Auth.Dal;
using Auth.Helpers.Interfaces;
using Auth.Interfaces;

namespace Auth.Helpers.Extensions;

public static class DependencyInjection
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<IDataValidationService, DataValidationService>();
        services.AddScoped<IPasswordHashingService, PasswordHashingService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IUserService, UserService>();
    }
}