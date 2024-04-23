using MenCore.Persistence.Repositories;
using RentACar.Application.Services.Repositories;
using RentACar.Domaim.Entities;
using RentACar.Persistence.Contexts;

namespace RentACar.Persistence.Repositories;

public class CarRepository : EfRepositoryBase<Car, Guid, BaseDatabaseContext>, ICarRepository
{
    public CarRepository(BaseDatabaseContext context) : base(context)
    {
    }
}