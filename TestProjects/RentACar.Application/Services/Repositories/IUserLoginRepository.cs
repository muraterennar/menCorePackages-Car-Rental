using MenCore.Persistence.Repositories;
using RentACar.Domaim.Entities;

namespace RentACar.Application.Services.Repositories;

public interface IUserLoginRepository : IAsyncRepository<UserLogin, int>, IRepository<UserLogin, int>
{
}