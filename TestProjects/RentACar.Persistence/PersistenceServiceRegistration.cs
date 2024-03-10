﻿using Microsoft.EntityFrameworkCore;
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
        services.AddDbContext<BaseDatabaseContext>(options => options.UseSqlServer(configuration["ConnectionStrings:mssqlserverTest"], opt => opt.EnableRetryOnFailure()));
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