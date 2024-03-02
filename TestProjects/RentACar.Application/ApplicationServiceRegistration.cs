using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace RentACar.Application;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices (this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
        });

        return services;
    }
}