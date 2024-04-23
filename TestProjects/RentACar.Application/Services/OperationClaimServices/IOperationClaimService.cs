using MenCore.Persistence.Paging;
using MenCore.Security.Entities;

namespace RentACar.Application.Services.OperationClaimServices;

public interface IOperationClaimService
{
    Task<Paginate<OperationClaim>> GetAllAsync();
    Task<OperationClaim> GetByIdAsync(int id);
    Task<OperationClaim> GetByNameAsync(string name);
    Task<OperationClaim> CreateAsync(OperationClaim operationClaim);
    Task<OperationClaim> UpdateAsync(OperationClaim operationClaim);
    Task<OperationClaim> DeleteAsync(OperationClaim operationClaim);
}