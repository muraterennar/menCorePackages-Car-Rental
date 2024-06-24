using MenCore.Persistence.Repositories;
using RentACar.Application.Services.Repositories;
using RentACar.Domaim.Entities;
using RentACar.Persistence.Contexts;

namespace RentACar.Persistence.Repositories;

public class UserLoginRepository : EfRepositoryBase<UserLogin, int, BaseDatabaseContext>, IUserLoginRepository
{
    public UserLoginRepository(BaseDatabaseContext context) : base(context)
    {
    }
}