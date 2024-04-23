using MenCore.Persistence.Repositories;
using MenCore.Security.Entities;

namespace RentACar.Application.Services.Repositories;

public interface IUserOperationClaimRepository : IAsyncRepository<UserOperationClaim, int>,
    IRepository<UserOperationClaim, int>
{
}