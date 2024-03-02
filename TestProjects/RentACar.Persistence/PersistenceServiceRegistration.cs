using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RentACar.Application.Services.Repositories.BrandRepositories;
using RentACar.Persistence.Contexts;
using RentACar.Persistence.Repositories;

namespace RentACar.Persistence;

public static class PersistenceServiceRegistration
{
    public static IServiceCollection AddPersistenceServices (this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<BaseDbContext>(options =>
        {
            options.UseInMemoryDatabase("MenCore");
        });
        services.AddScoped<IBrandRepository, BrandRepository>();
        return services;
    }
}