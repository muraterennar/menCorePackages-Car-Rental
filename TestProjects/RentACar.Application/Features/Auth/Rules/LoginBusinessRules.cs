using MenCore.Application.Rules;
using MenCore.CrossCuttingConserns.Exceptions.Types;
using MenCore.Security.Entities;
using MenCore.Security.Hashing;
using RentACar.Application.Features.Auth.Constants;
using RentACar.Application.Services.Repositories;

namespace RentACar.Application.Features.Auth.Rules;

public class LoginBusinessRules : BaseBusinessRules
{
    private readonly IUserRepository _userRepository;

    public LoginBusinessRules(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    // Kullanıcı şifresinin eşleşip eşleşmediğini kontrol eder.
    public Task UserPasswordShouldBeMatched(User user, string password)
    {
        if (!HashingHelper.VerifyPassswordHash(password, user.PasswordHash, user.PasswordSalt))
            throw new AuthorizationException(AuthMessages.PasswordDontMatch);
        return Task.CompletedTask;
    }

    public async Task UserUsernameShouldBeMatched(string username)
    {
        bool user = await _userRepository.AnyAsync(u => u.Username == username);
        if (!user)
            throw new AuthorizationException(AuthMessages.UserDontExists);
    }

    public async Task UserEmailShouldBeMatched(string email)
    {
        bool user = await _userRepository.AnyAsync(u => u.Email == email);
        if (!user)
            throw new AuthorizationException(AuthMessages.UserDontExists);
    }

    public async Task IsPasswordVerified(User user, string password)
    {
        bool result = HashingHelper.VerifyPassswordHash(password, user.PasswordHash, user.PasswordSalt);
        if (!result)
            throw new AuthorizationException(AuthMessages.PasswordDontMatch);
    }
}