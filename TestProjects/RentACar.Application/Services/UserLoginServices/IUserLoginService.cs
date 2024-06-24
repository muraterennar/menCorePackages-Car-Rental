using RentACar.Domaim.Entities;

namespace RentACar.Application.Services.UserLoginServices;

public interface IUserLoginService
{
    public Task<bool> CheckGooleLoginAsync(string loginProvider, string providerKey);
    public Task<UserLogin> GetGooleLoginAsync(string loginProvider, string providerKey);

    public Task<UserLogin> CreateAsync(UserLogin userLogin);
    public Task<UserLogin> UpdateAsync(UserLogin userLogin);
    public Task<UserLogin> DeleteAsync(UserLogin userLogin);
}