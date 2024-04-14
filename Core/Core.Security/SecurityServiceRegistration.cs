using MenCore.Security.EmailAuthenticator;
using MenCore.Security.JWT;
using MenCore.Security.OtpAuthenticator;
using MenCore.Security.OtpAuthenticator.OtpNet;
using Microsoft.Extensions.DependencyInjection;

namespace MenCore.Security;

public static class SecurityServiceRegistration
{
    public static IServiceCollection AddSecurityService(this IServiceCollection services)
    {
        services.AddScoped<ITokenHelper, JwtHelper>();
        services.AddScoped<IEmailAuthenticatorHelper, EmailAuthenticatorHelper>();
        services.AddScoped<IOtpAuthenticatorHelper, OtpNetOtpAuthenticatorHelper>();

        return services;
    }
}