using MenCore.Security.Entities;

namespace RentACar.Application.Services.PasswordResetServices;

public interface IPasswordResetService
{
    public Task<PasswordReset> CreateResetTokenAsync(User user);
    
    public Task<bool> ResetPasswordAsync(User user, string token, string newPassword);
    
    public Task<bool> VerifyResetTokenAsync(User user, string token);
    
    public Task<bool> DeleteResetTokenAsync(PasswordReset user);
}