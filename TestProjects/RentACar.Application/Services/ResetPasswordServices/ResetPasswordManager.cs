using RentACar.Application.Services.Repositories;
using MenCore.Security.JWT;
using RentACar.Application.Services.AuthServices;

namespace RentACar.Application.Services.ResetPasswordServices;

public class ResetPasswordManager:IResetPasswordService
{
    private readonly IUserRepository _userRepository;
    private readonly IAuthService _authService;
    private readonly ITokenHelper _tokenHelper;

    public ResetPasswordManager(IUserRepository userRepository, IAuthService authService, ITokenHelper tokenHelper)
    {
        _userRepository = userRepository;
        _authService = authService;
        _tokenHelper = tokenHelper;
    }

    public Task<string> GenerateResetPasswordTokenAsync()
    {
        var token = Guid.NewGuid();
        return Task.FromResult(token.ToString());

    }
}