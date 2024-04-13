using MenCore.Security.JWT;

namespace RentACar.Application.Features.Auth.Commands.Login;

public class LoginResponse
{
    public string Token { get; set; }
    public DateTime ExpirationDate { get; set; }
}