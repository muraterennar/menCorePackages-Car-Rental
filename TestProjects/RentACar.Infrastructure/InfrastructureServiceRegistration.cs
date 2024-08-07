using MenCore.Mailing.MailKitImplementations;
using Microsoft.Extensions.DependencyInjection;
using RentACar.Infrastructure.Auths.GoogleLogin;
using RentACar.Infrastructure.Mail;
using RentACar.Infrastructure.Mail.GeneratedTemplates;
using RentACar.Infrastructure.Mail.GeneratedTemplates.EnableMailAuthenticator;
using RentACar.Infrastructure.Mail.GeneratedTemplates.PasswordResetMailAuthenticator;

namespace RentACar.Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureService(this IServiceCollection services)
    {
        services.AddScoped<IEnableEmailAuthenticatorTemplate, EnableEmailAuthenticatorTemplate>();
        services.AddScoped<IPasswordResetMailAuthenticatorTemplate, PasswordResetMailAuthenticatorTemplate>();
        services.AddScoped<IGoogleAuthService, GoogleAuthManager>();

        return services;
    }
}