using MenCore.Mailing.MailKitImplementations;
using Microsoft.Extensions.DependencyInjection;

namespace MenCore.Mailing;

public static class MailingRegistrationService
{
    public static IServiceCollection AddMailingService(this IServiceCollection services)
    {
        services.AddScoped<IMailService, MailKitMailService>();

        return services;
    }
}