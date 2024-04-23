using MenCore.Persistence.Repositories;
using RentACar.Application.Services.Repositories;
using RentACar.Domaim.Entities;
using RentACar.Persistence.Contexts;

namespace RentACar.Persistence.Repositories;

public class FuelRepository : EfRepositoryBase<Fuel, Guid, BaseDatabaseContext>, IFuelRepository
{
    public FuelRepository(BaseDatabaseContext context) : base(context)
    {
    }
}