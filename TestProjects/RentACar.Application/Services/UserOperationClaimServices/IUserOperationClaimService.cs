using MenCore.Application.Request;
using MenCore.Persistence.Paging;
using MenCore.Security.Entities;

namespace RentACar.Application.Services.UserOperationClaimServices;

public interface IUserOperationClaimService
{
    Task<Paginate<UserOperationClaim>> GetAllAsync(PageRequest pageRequest);
    Task<Paginate<UserOperationClaim>> IncludableGetAllAsync(PageRequest pageRequest);
    Task<UserOperationClaim> GetByIdAsync(int id);
    Task<UserOperationClaim> GetByUserIdAsync(int userId);
    Task<UserOperationClaim> GetByOperationClaimIdAsync(int operationClaimId);
    Task<UserOperationClaim> CreateAsync(UserOperationClaim userOperationClaim);
    Task<UserOperationClaim> UpdateAsync(UserOperationClaim userOperationClaim);
    Task<UserOperationClaim> DeleteAsync(UserOperationClaim userOperationClaim);
}