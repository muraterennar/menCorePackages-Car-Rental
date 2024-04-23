using MenCore.Persistence.Repositories;
using MenCore.Security.Entities;

namespace RentACar.Application.Services.Repositories;

public interface IRefreshTokenRepository : IAsyncRepository<RefreshToken, int>, IRepository<RefreshToken, int>
{
}