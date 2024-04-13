using MenCore.Persistence.Repositories;
using MenCore.Security.Entities;
using RentACar.Application.Services.Repositories;
using RentACar.Persistence.Contexts;

namespace RentACar.Persistence.Repositories;

public class OperationClaimRepository : EfRepositoryBase<OperationClaim, int, BaseDatabaseContext>, IOperationClaimRepository
{
    public OperationClaimRepository(BaseDatabaseContext context) : base(context)
    {
    }
}