using MenCore.Persistence.Repositories;
using RentACar.Domaim.Entities;

namespace RentACar.Application.Services.Repositories;

public interface ICarRepository : IAsyncRepository<Car, Guid>, IRepository<Car, Guid>
{
}