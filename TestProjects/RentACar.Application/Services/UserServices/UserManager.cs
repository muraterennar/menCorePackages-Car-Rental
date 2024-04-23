using MenCore.Security.Entities;
using RentACar.Application.Services.Repositories;

namespace RentACar.Application.Services.UserServices;

public class UserManager : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserManager(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        var user = await _userRepository.GetAsync(u => u.Email == email);
        return user;
    }

    public async Task<User> GetByIdAsync(int id)
    {
        var user = await _userRepository.GetAsync(u => u.Id == id);
        return user ?? new User();
    }


    public async Task<User?> GetByUsernameAsync(string username)
    {
        var user = await _userRepository.GetAsync(u => u.Username == username);
        return user;
    }

    public async Task<User> CreateAsync(User user)
    {
        var createdUser = await _userRepository.AddAsync(user);
        return createdUser;
    }

    public async Task<User> UpdateAsync(User user)
    {
        var updatedUser = await _userRepository.UpdateAsync(user);
        return updatedUser;
    }

    public async Task<User> DeleteAsync(User user)
    {
        var deletedUser = await _userRepository.DeleteAsync(user);
        return deletedUser;
    }
}