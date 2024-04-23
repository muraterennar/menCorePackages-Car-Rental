using MenCore.Persistence.Repositories;
using MenCore.Security.Entities;
using RentACar.Application.Services.Repositories;
using RentACar.Persistence.Contexts;

namespace RentACar.Persistence.Repositories;

public class UserOperationClaimRepository : EfRepositoryBase<UserOperationClaim, int, BaseDatabaseContext>,
    IUserOperationClaimRepository
{
    public UserOperationClaimRepository(BaseDatabaseContext context) : base(context)
    {
    }
}