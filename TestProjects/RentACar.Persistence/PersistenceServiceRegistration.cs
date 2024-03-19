using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RentACar.Application.Services.Repositories;
using RentACar.Persistence.Contexts;
using RentACar.Persistence.Repositories;

namespace RentACar.Persistence;

public static class PersistenceServiceRegistration
{
    public static IServiceCollection AddPersistenceServices (this IServiceCollection services, IConfiguration configuration)
    {
        // --- Database InMemory
        services.AddDbContext<BaseDatabaseContext>(opt => opt.UseInMemoryDatabase("InMemoryDatabase"));
        // --- Database SQL Server
        services.AddDbContext<BaseDatabaseContext>(options => options.UseSqlServer(configuration["ConnectionStrings:mssqlserverTest"]));

        services.AddScoped<IBrandRepository, BrandRepository>();
        services.AddScoped<IModelRepository, ModelRepository>();
        services.AddScoped<ICarRepository, CarRepository>();
        services.AddScoped<IFuelRepository, FuelRepository>();
        services.AddScoped<ITransmissionRepository, TransmissionRepository>();


        // --- Data Seeding
        var seedData = new SeedDatas();
        seedData.SeedDataAsync(configuration).GetAwaiter().GetResult();

        return services;
    }
}