using MenCore.Persistence.Repositories;
using MenCore.Security.Entities;

namespace RentACar.Application.Services.Repositories;

public interface IUserRepository : IAsyncRepository<User, int>, IRepository<User, int>
{
}