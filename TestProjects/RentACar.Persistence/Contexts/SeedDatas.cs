using MenCore.Security.Entities;
using MenCore.Security.Hashing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RentACar.Application.Features.Users.Constants;
using RentACar.Domaim.Entities;

namespace RentACar.Persistence.Contexts;

public class SeedDatas
{
    private List<Brand> SeedBrands()
    {
        List<Brand> brands = new()
        {
            new Brand {Id= Guid.Parse("917d46b7-274f-4abd-af70-df8d2086993b"), BrandName = "togg", CreatedDate = DateTime.UtcNow},
            new Brand {Id= Guid.Parse("026423a3-a5cd-4485-9321-af9283204f44"), BrandName = "tesla", CreatedDate = DateTime.UtcNow},
            new Brand {Id= Guid.Parse("73c99b29-8f6b-4727-820a-9334bf814827"), BrandName = "bmw", CreatedDate = DateTime.UtcNow},
            new Brand {Id= Guid.Parse("c9c598cb-8358-4cab-8101-8896892b2c7f"), BrandName = "mercedes-benz", CreatedDate = DateTime.UtcNow},
            new Brand {Id= Guid.Parse("94f00a96-a0a7-43a7-97a3-ccc89880bbb7"), BrandName = "renault", CreatedDate = DateTime.UtcNow},
            new Brand {Id= Guid.Parse("bca40192-3ed0-4596-ae51-fafa16c81c29"), BrandName = "fiat", CreatedDate = DateTime.UtcNow},
            new Brand {Id= Guid.Parse("36cdaceb-1e8d-43fa-8556-ac2d2bf75d01"), BrandName = "honda", CreatedDate = DateTime.UtcNow},
            new Brand {Id= Guid.Parse("9b2b9e00-b865-4937-a9bf-d06cc5cc9028"), BrandName = "hundai", CreatedDate = DateTime.UtcNow},
            new Brand {Id= Guid.Parse("4f0b276c-b583-42a7-bb6d-b88b58bab090"), BrandName = "ford", CreatedDate = DateTime.UtcNow}
        };

        return brands;
    }

    private List<Fuel> SeedFuels()
    {
        List<Fuel> fuels = new()
        {
            new Fuel{Id = Guid.Parse("d4945194-4d2a-4f79-960f-465c8d02a8c9"), FuelName = "petrol", CreatedDate = DateTime.UtcNow},
            new Fuel{Id = Guid.Parse("d9ad566d-5c67-4ccb-95b3-a421cabc686b"), FuelName = "diesel", CreatedDate = DateTime.UtcNow},
            new Fuel{Id = Guid.Parse("4231648f-5fab-4156-88a8-e72a9028004d"), FuelName = "electric", CreatedDate = DateTime.UtcNow}
        };

        return fuels;
    }

    private List<Transmission> SeedTransmissions()
    {
        List<Transmission> transmissions = new()
        {
            new Transmission{Id=Guid.Parse("b290b7b2-ec8b-41a1-8c23-5f1ee17a5003"), TransmissionName = "automatic", CreatedDate = DateTime.UtcNow},
            new Transmission{Id=Guid.Parse("18156649-8e7b-4a82-9208-1ceef2709feb"), TransmissionName = "semi-automatic", CreatedDate = DateTime.UtcNow},
            new Transmission{Id=Guid.Parse("83d24f59-2102-48ea-b30a-9fa28ee65808"), TransmissionName = "manuel", CreatedDate = DateTime.UtcNow}
        };

        return transmissions;
    }

    private List<Model> SeedModels()
    {
        List<Model> models = new()
        {
            new Model{Id=Guid.Parse("493a5748-00d3-484c-871f-e8806774058e"), BrandId=Guid.Parse("917d46b7-274f-4abd-af70-df8d2086993b"), TransmissionId = Guid.Parse("b290b7b2-ec8b-41a1-8c23-5f1ee17a5003"), FuelId=Guid.Parse("4231648f-5fab-4156-88a8-e72a9028004d"), ModelName = "t10x", DailyPrice = 2000, ImageUrl = "https://placehold.co/550x400", CreatedDate = DateTime.UtcNow},
            new Model{Id=Guid.Parse("40c9b89c-9960-4b6e-9089-920c49eb12f0"), BrandId=Guid.Parse("026423a3-a5cd-4485-9321-af9283204f44"), TransmissionId = Guid.Parse("b290b7b2-ec8b-41a1-8c23-5f1ee17a5003"), FuelId=Guid.Parse("4231648f-5fab-4156-88a8-e72a9028004d"), ModelName = "model s", DailyPrice = 1800, ImageUrl = "https://placehold.co/550x400", CreatedDate = DateTime.UtcNow}
        };

        return models;
    }

    private List<Car> SeedCars()
    {
        List<Car> cars = new()
        {
            new Car{Id=Guid.Parse("fc1cbff0-a1ca-428e-b62f-c391ef5cb3d5"), ModelId = Guid.Parse("493a5748-00d3-484c-871f-e8806774058e"), Kilometer = 5500, ModelYear = 2023, Plate = "34 MN 0653", CarState = Domaim.Enums.CarState.Avaible, CreatedDate = DateTime.UtcNow},
            new Car{Id=Guid.Parse("dd581852-a67d-47a6-8474-620031a368d4"), ModelId = Guid.Parse("493a5748-00d3-484c-871f-e8806774058e"), Kilometer = 4000, ModelYear = 2023, Plate = "53 MEN 2804", CarState = Domaim.Enums.CarState.Avaible, CreatedDate = DateTime.UtcNow},
            new Car{Id=Guid.Parse("6505ac77-243d-4392-84d3-5803bffc372e"), ModelId = Guid.Parse("40c9b89c-9960-4b6e-9089-920c49eb12f0"), Kilometer = 200, ModelYear = 2020, Plate = "06 NG 0423", CarState = Domaim.Enums.CarState.Avaible, CreatedDate = DateTime.UtcNow}
        };

        return cars;
    }

    private List<OperationClaim> SeedOperationClaims()
    {
        List<OperationClaim> operationClaims = new()
        {
            new OperationClaim { Name = UserOperationClaims.Admin, CreatedDate = DateTime.UtcNow },
            new OperationClaim { Name = UserOperationClaims.Read, CreatedDate = DateTime.UtcNow },
            new OperationClaim { Name = UserOperationClaims.Write, CreatedDate = DateTime.UtcNow },
            new OperationClaim { Name = UserOperationClaims.Add, CreatedDate = DateTime.UtcNow },
            new OperationClaim { Name = UserOperationClaims.Update, CreatedDate = DateTime.UtcNow },
            new OperationClaim { Name = UserOperationClaims.Delete, CreatedDate = DateTime.UtcNow }
        };

        return operationClaims;
    }

    private List<User> SeedUsers()
    {
        HashingHelper.CreatePasswordHash(
        password: "Deneme1234!",
        passwordHash: out byte[] passwordHash,
        passwordSalt: out byte[] passwordSalt
        );

        List<User> users = new()
        {
            new User {
                FirstName = "admin",
                LastName = "mencoretech",
                Email = "admin.@admin.com",
                Username = "admin",
                Status = true,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            }
        };

        return users;
    }

    private List<UserOperationClaim> SeedUserOperationClaims()
    {
        List<UserOperationClaim> userOperationClaims = new()
        {
            new UserOperationClaim{OperationClaimId = 1, UserId = 1, CreatedDate = DateTime.UtcNow}
        };

        return userOperationClaims;

    }

    public async Task SeedDataAsync(IConfiguration configuration)
    {
        var dbContextBuiler = new DbContextOptionsBuilder();

        dbContextBuiler.UseSqlServer(configuration["ConnectionStrings:mssqlserverTest"]);

        var context = new BaseDatabaseContext(dbContextBuiler.Options);

        if (!context.Brands.Any())
        {
            // marka Ekleme operasyonları

            var seedBrand = SeedBrands();
            await context.Brands.AddRangeAsync(seedBrand);
            await context.SaveChangesAsync();
        }

        if (!context.Transmissions.Any())
        {
            // transmission ekleme operasyonları

            var seedTransmission = SeedTransmissions();
            await context.Transmissions.AddRangeAsync(seedTransmission);
            await context.SaveChangesAsync();
        }

        if (!context.Fuels.Any())
        {
            // fuel ekleme operasyonları
            var seedFuel = SeedFuels();
            await context.Fuels.AddRangeAsync(seedFuel);
            await context.SaveChangesAsync();
        }

        if (!context.Models.Any())
        {
            // models ekleme operasyonları

            var seedModel = SeedModels();
            await context.Models.AddRangeAsync(seedModel);
            await context.SaveChangesAsync();
        }

        if (!context.Cars.Any())
        {
            // cars ekleme operasyonları

            var seedCar = SeedCars();
            await context.Cars.AddRangeAsync(seedCar);
            await context.SaveChangesAsync();
        }

        if (!context.Users.Any())
        {
            // Sisteme Admin Ekleme

            var seedUser = SeedUsers();
            await context.Users.AddRangeAsync(seedUser);
            await context.SaveChangesAsync();
        }

        if (!context.OperationClaims.Any())
        {
            var seedOperationCliams = SeedOperationClaims();
            await context.OperationClaims.AddRangeAsync(seedOperationCliams);
            await context.SaveChangesAsync();
        }

        if (!context.UserOperationClaims.Any())
        {
            var seedUserOperationClaims = SeedUserOperationClaims();
            await context.UserOperationClaims.AddRangeAsync(seedUserOperationClaims);
            await context.SaveChangesAsync();
        }

        try
        {
            await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
}