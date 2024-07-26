using MenCore.Security.Entities;
using MenCore.Security.JWT;

namespace RentACar.Application.Features.Auth.Commands.GoogleLogin;

public class GoogleLoginResponse
{
    public string Token { get; set; }
    public DateTime Expiration { get; set; }
}