using MenCore.Security.Entities;
using RentACar.Application.Services.Repositories;
using RentACar.Application.Services.UserServices;

namespace RentACar.Application.Services.PasswordResetServices;

public class PasswordResetManager : IPasswordResetService
{
    private readonly IUserService _userService;
    private readonly IPasswordResetRepository _passwordResetRepository;

    public PasswordResetManager(IUserService userService, IPasswordResetService passwordResetService, IPasswordResetRepository passwordResetRepository)
    {
        _userService = userService;
        _passwordResetRepository = passwordResetRepository;
    }

    public Task<PasswordReset> CreateResetTokenAsync(User user)
    {
        string token = GenerateUrlSafeToken();
        
        PasswordReset passwordReset = new()
        {
            UserId = user.Id,
            Token = token,
            ExpiryDate = DateTime.UtcNow.AddMinutes(10)
        };
        
        return Task.FromResult(passwordReset);
    }

    public async Task<bool> ResetPasswordAsync(User user, string token, string newPassword)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> VerifyResetTokenAsync(User user, string token)
    {
        var getToken = await  _passwordResetRepository.GetAsync(p=>p.Id == user.Id && p.Token == token);
        return getToken != null;
    }

    public async Task<bool> DeleteResetTokenAsync(PasswordReset passwordReset)
    {
        var deletedToken = await _passwordResetRepository.DeleteAsync(passwordReset);
        if(deletedToken == null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    
    private static string GenerateUrlSafeToken()
    {
        // Rastgele bir UUID (GUID) oluştur
        Guid guid = Guid.NewGuid();
        
        // UUID'yi byte dizisine dönüştür
        byte[] bytes = guid.ToByteArray();
        
        // Byte dizisini Base64 stringine dönüştür
        string base64Token = Convert.ToBase64String(bytes);
        
        // Base64 stringindeki '+' ve '/' karakterlerini URL dostu karakterlerle değiştir
        // ve '=' dolgu karakterini kaldır
        string urlSafeToken = base64Token.Replace('+', '-')
            .Replace('/', '_')
            .TrimEnd('=');
        
        return urlSafeToken;
    }
}