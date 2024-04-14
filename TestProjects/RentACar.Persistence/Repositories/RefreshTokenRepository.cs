using MenCore.Persistence.Repositories;
using MenCore.Security.Entities;
using RentACar.Application.Services.Repositories;
using RentACar.Persistence.Contexts;

namespace RentACar.Persistence.Repositories;

public class RefreshTokenRepository : EfRepositoryBase<RefreshToken, int, BaseDatabaseContext>, IRefreshTokenRepository
{
    public RefreshTokenRepository(BaseDatabaseContext context) : base(context)
    {
    }
}