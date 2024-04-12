using MenCore.Application.Rules;
using MenCore.CrossCuttingConserns.Exceptions.Types;
using MenCore.Security.Entities;
using MenCore.Security.Hashing;
using RentACar.Application.Features.Auth.Constants;
using RentACar.Application.Services.Repositories;

namespace RentACar.Application.Features.Users.Rules;

public class UserBusinessRules : BaseBusinessRules
{
    private readonly IUserRepository _userRepository;

    public UserBusinessRules(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    // Seçilen kullanıcının varlığını kontrol eder.
    public Task UserShouldBeExistWhenSelected(User? user)
    {
        if (user == null)
            throw new BusinessException(AuthMessages.UserDontExists);

        return Task.CompletedTask;
    }

    // Belirtilen kullanıcı kimliğinin varlığını kontrol eder.
    public async Task UserIdShouldBeExistWhenSelected(int id)
    {
        bool doesExist = await _userRepository.AnyAsync(predicate: u => u.Id == id, enableTracking: false);
        if (!doesExist)
            throw new BusinessException(AuthMessages.UserDontExists);
    }

    // Kullanıcı şifresinin eşleşip eşleşmediğini kontrol eder.
    public Task UserPasswordShouldBeMatched(User user, string password)
    {
        if (!HashingHelper.VerifyPassswordHash(password, user.PasswordHash, user.PasswordSalt))
            throw new BusinessException(AuthMessages.PasswordDontMatch);
        return Task.CompletedTask;
    }

    // E-posta adresinin veritabanında olmadığını kontrol eder.
    public async Task UserEmailShouldNotExistWhenInsert(string email)
    {
        bool doesExist = await _userRepository.AnyAsync(predicate: u => u.Email == email, enableTracking: false);
        if (doesExist)
            throw new BusinessException(AuthMessages.UserMailAlreadyExists);
    }
}
