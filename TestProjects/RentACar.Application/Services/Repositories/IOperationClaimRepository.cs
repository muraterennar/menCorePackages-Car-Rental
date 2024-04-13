using MenCore.Persistence.Repositories;
using MenCore.Security.Entities;

namespace RentACar.Application.Services.Repositories;

public interface IOperationClaimRepository : IAsyncRepository<OperationClaim, int>, IRepository<OperationClaim, int>
{

}