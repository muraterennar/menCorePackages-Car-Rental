using MenCore.Security.Entities;

namespace RentACar.Application.Services.UserServices;

public interface IUserService
{
    public Task<User?> GetByEmailAsync(string email);
    public Task<User?> GetByUsernameAsync(string username);
    public Task<User> GetByIdAsync(int id);
    public Task<User> CreateAsync(User user);
    public Task<User> UpdateAsync(User user);
    public Task<User> DeleteAsync(User user);
}