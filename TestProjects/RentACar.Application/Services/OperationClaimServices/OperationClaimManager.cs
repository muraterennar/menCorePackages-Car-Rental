using MenCore.Persistence.Paging;
using MenCore.Security.Entities;
using RentACar.Application.Services.Repositories;

namespace RentACar.Application.Services.OperationClaimServices;

public class OperationClaimManager : IOperationClaimService
{
    public readonly IOperationClaimRepository _operationClaimRepository;

    public OperationClaimManager(IOperationClaimRepository operationClaimRepository)
    {
        _operationClaimRepository = operationClaimRepository;
    }

    public async Task<Paginate<OperationClaim>> GetAllAsync()
    {
        var operationClaims = await _operationClaimRepository.GetListAsync();
        return operationClaims;
    }

    public async Task<OperationClaim> GetByIdAsync(int id)
    {
        var operationClaim = await _operationClaimRepository.GetAsync(o => o.Id == id);
        return operationClaim ?? new OperationClaim();
    }

    public async Task<OperationClaim> GetByNameAsync(string name)
    {
        var operationClaim = await _operationClaimRepository.GetAsync(o => o.Name == name);
        return operationClaim ?? new OperationClaim();
    }

    public async Task<OperationClaim> CreateAsync(OperationClaim operationClaim)
    {
        var createdOperationClaim = await _operationClaimRepository.AddAsync(operationClaim);
        return createdOperationClaim;
    }

    public async Task<OperationClaim> UpdateAsync(OperationClaim operationClaim)
    {
        var updatedOperationClaim = await _operationClaimRepository.UpdateAsync(operationClaim);
        return updatedOperationClaim;
    }

    public async Task<OperationClaim> DeleteAsync(OperationClaim operationClaim)
    {
        var deletedOperationClaim = await _operationClaimRepository.DeleteAsync(operationClaim);
        return deletedOperationClaim;
    }
}