using MenCore.Security.Entities;

namespace RentACar.Application.Services.ResetPasswordServices;

public interface IResetPasswordService
{
    public Task<string> GenerateResetPasswordTokenAsync();
}