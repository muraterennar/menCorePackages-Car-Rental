using MenCore.Application.Rules;
using MenCore.CrossCuttingConserns.Exceptions.Types;
using RentACar.Application.Features.OperationClaims.Constants;
using RentACar.Application.Services.Repositories;

namespace RentACar.Application.Features.OperationClaims.Rules;

public class OperationClaimBusinessRules : BaseBusinessRules
{
    public readonly IOperationClaimRepository _operationClaimRepository;

    public OperationClaimBusinessRules(IOperationClaimRepository operationClaimRepository)
    {
        _operationClaimRepository = operationClaimRepository;
    }

    public async Task IsOperationClaimExist(string name)
    {
        var operationClaim = await _operationClaimRepository.GetAsync(u => u.Name == name);
        if (operationClaim == null) throw new BusinessException(OperationClaimMessages.OperationClaimNotFound);
    }

    public async Task IsOperationClaimNotFound(string name)
    {
        var operationClaim = await _operationClaimRepository.GetAsync(u => u.Name == name);
        if (operationClaim != null) throw new BusinessException(OperationClaimMessages.OperationClaimAlreadyExist);
    }
}