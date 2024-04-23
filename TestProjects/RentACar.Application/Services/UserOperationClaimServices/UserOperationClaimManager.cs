using MenCore.Application.Request;
using MenCore.Persistence.Paging;
using MenCore.Security.Entities;
using Microsoft.EntityFrameworkCore;
using RentACar.Application.Services.Repositories;

namespace RentACar.Application.Services.UserOperationClaimServices;

public class UserOperationClaimManager : IUserOperationClaimService
{
    private readonly IUserOperationClaimRepository _userOperationClaimRepository;

    public UserOperationClaimManager(IUserOperationClaimRepository userOperationClaimRepository)
    {
        _userOperationClaimRepository = userOperationClaimRepository;
    }

    public async Task<Paginate<UserOperationClaim>> GetAllAsync(PageRequest pageRequest)
    {
        var userOperationClaims = await _userOperationClaimRepository.GetListAsync
        (
            index: pageRequest.PageIndex,
            size: pageRequest.PageSize
        );
        return userOperationClaims;
    }

    public async Task<Paginate<UserOperationClaim>> IncludableGetAllAsync(PageRequest pageRequest)
    {
        var userOperationClaims = await _userOperationClaimRepository.GetListAsync(
            include: u => u.Include(u => u.User).Include(u => u.OperationClaim),
            index: pageRequest.PageIndex,
            size: pageRequest.PageSize
        );
        return userOperationClaims;
    }

    public async Task<UserOperationClaim> GetByIdAsync(int id)
    {
        var userOperationClaim = await _userOperationClaimRepository.GetAsync(u => u.Id == id,
            u => u.Include(u => u.User).Include(u => u.OperationClaim));
        return userOperationClaim ?? new UserOperationClaim();
    }

    public async Task<UserOperationClaim> GetByUserIdAsync(int userId)
    {
        var userOperationClaim = await _userOperationClaimRepository.GetAsync(u => u.UserId == userId,
            u => u.Include(u => u.User).Include(u => u.OperationClaim));
        return userOperationClaim ?? new UserOperationClaim();
    }

    public async Task<UserOperationClaim> GetByOperationClaimIdAsync(int operationClaimId)
    {
        var userOperationClaim = await _userOperationClaimRepository.GetAsync(
            u => u.OperationClaimId == operationClaimId,
            u => u.Include(u => u.User).Include(u => u.OperationClaim));
        return userOperationClaim ?? new UserOperationClaim();
    }

    public async Task<UserOperationClaim> CreateAsync(UserOperationClaim userOperationClaim)
    {
        var userOperationClaimResult = await _userOperationClaimRepository.AddAsync(userOperationClaim);
        return userOperationClaimResult;
    }

    public async Task<UserOperationClaim> UpdateAsync(UserOperationClaim userOperationClaim)
    {
        var userOperationClaimResult =
            await _userOperationClaimRepository.UpdateAsync(userOperationClaim);
        return userOperationClaimResult;
    }

    public async Task<UserOperationClaim> DeleteAsync(UserOperationClaim userOperationClaim)
    {
        var userOperationClaimResult =
            await _userOperationClaimRepository.DeleteAsync(userOperationClaim);
        return userOperationClaimResult;
    }
}