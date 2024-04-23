using MenCore.Application.Rules;
using MenCore.CrossCuttingConserns.Exceptions.Types;
using RentACar.Application.Features.UserOperationClaims.Constants;
using RentACar.Application.Services.Repositories;

namespace RentACar.Application.Features.UserOperationClaims.Rules;

public class UserOperationClaimBusinessRules : BaseBusinessRules
{
    private readonly IUserOperationClaimRepository _userOperationClaimRepository;

    public UserOperationClaimBusinessRules(IUserOperationClaimRepository userOperationClaimRepository)
    {
        _userOperationClaimRepository = userOperationClaimRepository;
    }

    public async Task IsUserOperationClaimExist(int id)
    {
        var userOperationClaim = await _userOperationClaimRepository.GetAsync(u => u.Id == id);
        if (userOperationClaim == null)
            throw new BusinessException(UserOperationClaimMessages.IsUserOperationClaimNotFound);
    }

    public async Task IsUserOperationClaimNotExist(int id)
    {
        var userOperationClaim = await _userOperationClaimRepository.GetAsync(u => u.Id == id);
        if (userOperationClaim != null)
            throw new BusinessException(UserOperationClaimMessages.IsUserOperationClaimAlreadyExist);
    }

    public async Task IsUserOperationClaimExistForUser(int userId)
    {
        var userOperationClaim = await _userOperationClaimRepository.GetAsync(u => u.UserId == userId);
        if (userOperationClaim == null)
            throw new BusinessException(UserOperationClaimMessages.IsUserOperationClaimNotFoundForUser);
    }

    public async Task IsUserOperationClaimExistForOperationClaim(int operationClaimId)
    {
        var userOperationClaim =
            await _userOperationClaimRepository.GetAsync(u => u.OperationClaimId == operationClaimId);
        if (userOperationClaim == null)
            throw new BusinessException(UserOperationClaimMessages.IsUserOperationClaimNotFoundForOperationClaim);
    }

    public async Task IsUserOperationClaimExistForUserAndOperationClaim(int userId, int operationClaimId)
    {
        var userOperationClaim =
            await _userOperationClaimRepository.GetAsync(u =>
                u.UserId == userId && u.OperationClaimId == operationClaimId);
        if (userOperationClaim == null)
            throw new BusinessException(UserOperationClaimMessages
                .IsUserOperationClaimNotFoundForUserAndOperationClaim);
    }

    public async Task IsUserOperationClaimNotExistForUserAndOperationClaim(int userId, int operationClaimId)
    {
        var userOperationClaim =
            await _userOperationClaimRepository.GetAsync(u =>
                u.UserId == userId && u.OperationClaimId == operationClaimId);
        if (userOperationClaim != null)
            throw new BusinessException(UserOperationClaimMessages
                .IsUserOperationClaimAlreadyExistForUserAndOperationClaim);
    }
}