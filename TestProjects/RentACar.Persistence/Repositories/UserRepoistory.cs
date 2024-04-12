using MenCore.Persistence.Repositories;
using MenCore.Security.Entities;
using RentACar.Application.Services.Repositories;
using RentACar.Persistence.Contexts;

namespace RentACar.Persistence.Repositories;

public class UserRepoistory : EfRepositoryBase<User, int, BaseDatabaseContext>, IUserRepository
{
    public UserRepoistory(BaseDatabaseContext context) : base(context)
    {
    }
}