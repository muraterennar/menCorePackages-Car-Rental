using MenCore.Mailing.MailKitImplementations;
using Microsoft.Extensions.DependencyInjection;
using RentACar.Infrastructure.Mail;
using RentACar.Infrastructure.Mail.GeneratedTemplates;
using RentACar.Infrastructure.Mail.GeneratedTemplates.EnableMailAuthenticator;

namespace RentACar.Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureService(this IServiceCollection services)
    {
        services.AddScoped<IMailTemplateGeneratorService, EnableEmailAuthenticatorTemplate>();

        return services;
    }
}