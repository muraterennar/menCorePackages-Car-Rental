using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RentACar.Application.Services.Repositories;
using RentACar.Persistence.Contexts;
using RentACar.Persistence.Repositories;

namespace RentACar.Persistence;

public static class PersistenceServiceRegistration
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        // --- Database InMemory
        //services.AddDbContext<BaseDatabaseContext>(opt => opt.UseInMemoryDatabase("InMemoryDatabase"));
        // --- Database SQL Server
        services.AddDbContext<BaseDatabaseContext>(options =>
            options.UseSqlServer(configuration["ConnectionStrings:mssqlserverTest"]));

        services.AddScoped<IBrandRepository, BrandRepository>();
        services.AddScoped<IModelRepository, ModelRepository>();
        services.AddScoped<ICarRepository, CarRepository>();
        services.AddScoped<IFuelRepository, FuelRepository>();
        services.AddScoped<ITransmissionRepository, TransmissionRepository>();
        services.AddScoped<IUserRepository, UserRepoistory>();
        services.AddScoped<IOperationClaimRepository, OperationClaimRepository>();
        services.AddScoped<IUserOperationClaimRepository, UserOperationClaimRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IEmailAuthenticatorRepository, EmailAuthenticatorRepository>();
        services.AddScoped<IOtpAuthenticatorRepository, OtpAuthenticatorRepository>();


        // --- Data Seeding -----
        // Servis sağlayıcısını oluştur
        //TODO: Bu kısım hata verebilir. Çözüm bulunmalı.
        var serviceProvider = services.BuildServiceProvider();

        // DbContext nesnesini al
        var dbContext = serviceProvider.GetRequiredService<BaseDatabaseContext>();

        // Eğer DbContext InMemory veritabanı sağlayıcısını kullanmıyorsa
        if (!dbContext.Database.IsInMemory())
        {
            // SeedDataAsync metodunu kullanarak verileri ekle
            var seedData = new SeedDatas();
            seedData.SeedDataAsync(configuration).GetAwaiter().GetResult();
        }

        return services;
    }
}