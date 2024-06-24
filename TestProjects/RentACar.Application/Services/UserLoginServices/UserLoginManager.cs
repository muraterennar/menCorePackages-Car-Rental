using MenCore.CrossCuttingConserns.Exceptions.Types;
using RentACar.Application.Services.Repositories;
using RentACar.Domaim.Entities;

namespace RentACar.Application.Services.UserLoginServices;

public class UserLoginManager : IUserLoginService
{
    private readonly IUserLoginRepository _userLoginRepository;

    public UserLoginManager(IUserLoginRepository userLoginRepository)
    {
        _userLoginRepository = userLoginRepository;
    }

    public async Task<bool> CheckGooleLoginAsync(string loginProvider, string providerKey)
    {
        var userLogin =
            await _userLoginRepository.GetAsync(u => u.LoginProvider == loginProvider && u.ProviderKey == providerKey);
        return userLogin != null;
    }

    public async Task<UserLogin> GetGooleLoginAsync(string loginProvider, string providerKey)
    {
        var userLogin =
            await _userLoginRepository.GetAsync(u => u.LoginProvider == loginProvider && u.ProviderKey == providerKey);
        return userLogin;
    }

    public async Task<UserLogin> CreateAsync(UserLogin userLogin)
    {
        var createdUserLogin = await _userLoginRepository.AddAsync(userLogin);
        return createdUserLogin;
    }

    public async Task<UserLogin> UpdateAsync(UserLogin userLogin)
    {
        var updatedUserLogin = await _userLoginRepository.UpdateAsync(userLogin);
        return updatedUserLogin;
    }

    public async Task<UserLogin> DeleteAsync(UserLogin userLogin)
    {
        var deletedUserLogin = await _userLoginRepository.DeleteAsync(userLogin);
        return deletedUserLogin;
    }
}