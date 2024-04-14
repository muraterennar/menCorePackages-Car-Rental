using MenCore.Security.Entities;

namespace RentACar.Application.Services.UserServices;

public interface IUserService
{
    public Task<User?> GetByEmail(string email);
    public Task<User?> GetByUsername(string username);
    public Task<User> GetById(int id);
    public Task<User> Update(User user);
}