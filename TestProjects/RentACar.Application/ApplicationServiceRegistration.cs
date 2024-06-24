using System.Reflection;
using FluentValidation;
using MenCore.Application.Pipelines.Authorization;
using MenCore.Application.Pipelines.Caching;
using MenCore.Application.Pipelines.Logging;
using MenCore.Application.Pipelines.Transaction;
using MenCore.Application.Pipelines.Validation;
using MenCore.Application.Rules;
using MenCore.CrossCuttingConserns.Serilog;
using MenCore.CrossCuttingConserns.Serilog.Loggers;
using Microsoft.Extensions.DependencyInjection;
using RentACar.Application.Services.AuthenticatorServices;
using RentACar.Application.Services.AuthServices;
using RentACar.Application.Services.OperationClaimServices;
using RentACar.Application.Services.UserLoginServices;
using RentACar.Application.Services.UserOperationClaimServices;
using RentACar.Application.Services.UserServices;

namespace RentACar.Application;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        services.AddSubClassessOfType(Assembly.GetExecutingAssembly(), typeof(BaseBusinessRules));

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddSingleton<LoggerServiceBase, FileLogger>();
        services.AddSingleton<LoggerServiceBase, MsSqlServerLogger>();

        // Added Manager Classes
        services.AddScoped<IAuthService, AuthManager>();
        services.AddScoped<IUserService, UserManager>();
        services.AddScoped<IAuthenticatorService, AuthenticatorManager>();
        services.AddScoped<IOperationClaimService, OperationClaimManager>();
        services.AddScoped<IUserOperationClaimService, UserOperationClaimManager>();
        services.AddScoped<IUserLoginService, UserLoginManager>();

        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
            configuration.AddOpenBehavior(typeof(RequestValidationBehavior<,>));
            configuration.AddOpenBehavior(typeof(TransactionScopeBehavior<,>));
            configuration.AddOpenBehavior(typeof(CachingBehavior<,>));
            configuration.AddOpenBehavior(typeof(CacheRemovingBehavier<,>));
            configuration.AddOpenBehavior(typeof(LoggingBehavior<,>));
            configuration.AddOpenBehavior(typeof(AuthorizationBehavior<,>));
        });

        return services;
    }

    #region Tipi Verilen Türde Olan Herşeyi IoC ye Ekleyen Method

    public static IServiceCollection AddSubClassessOfType(this IServiceCollection services, Assembly assembly,
        Type type, Func<IServiceCollection, Type, IServiceCollection>? addWithLifeCycle = null)
    {
        // Verilen derleme içindeki tüm tipleri alır
        var types = assembly.GetTypes().Where(t => t.IsSubclassOf(type) && type != t).ToList();

        // Her bir alt tür için döngü
        foreach (var typ in types)
            // Belirtilen yaşam döngüsü işlevi varsa
            if (addWithLifeCycle == null)
                // Geçici olarak hizmet ekler
                services.AddScoped(typ);
            else
                // Belirtilen yaşam döngüsü işlevini çağırır
                addWithLifeCycle(services, typ);
        return services;
    }

    #endregion
}