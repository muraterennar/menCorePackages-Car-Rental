using System.Reflection;
using FluentValidation;
using MenCore.Application.Pipelines.Transaction;
using MenCore.Application.Pipelines.Validation;
using MenCore.Application.Rules;
using Microsoft.Extensions.DependencyInjection;

namespace RentACar.Application;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices (this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        services.AddSubClassessOfType(Assembly.GetExecutingAssembly(), typeof(BaseBusinessRules));

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
            configuration.AddOpenBehavior(typeof(RequestValidationBehavior<,>));
            configuration.AddOpenBehavior(typeof(TransactionScopeBehavior<,>));
        });

        return services;
    }

    #region Tipi Verilen Türde Olan Herşeyi IoC ye Ekleyen Method
    public static IServiceCollection AddSubClassessOfType (this IServiceCollection services, Assembly assembly, Type type, Func<IServiceCollection, Type, IServiceCollection>? addWithLifeCycle = null)
    {
        // Verilen derleme içindeki tüm tipleri alır
        var types = assembly.GetTypes().Where(t => t.IsSubclassOf(type) && type != t).ToList();

        // Her bir alt tür için döngü
        foreach (var typ in types)
        {
            // Belirtilen yaşam döngüsü işlevi varsa
            if (addWithLifeCycle == null)
                // Geçici olarak hizmet ekler
                services.AddScoped(typ);
            else
                // Belirtilen yaşam döngüsü işlevini çağırır
                addWithLifeCycle(services, typ);
        }
        return services;
    }
    #endregion
}