using MenCore.Persistence.Repositories;
using MenCore.Security.Entities;

namespace RentACar.Application.Services.Repositories;

public interface IPasswordResetRepository : IAsyncRepository<PasswordReset, int>, IRepository<PasswordReset, int>
{
}