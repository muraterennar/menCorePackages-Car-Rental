using MenCore.Security.Entities;
using MenCore.Security.JWT;

namespace RentACar.Application.Features.Auth.Commands.Register;

public class RegisteredResponse
{
    public AccessToken AccessToken { get; set; }
    public RefreshToken RefreshToken { get; set; }
}