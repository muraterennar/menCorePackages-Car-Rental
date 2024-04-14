namespace MenCore.Application.Dtos;

public class UserForLoginDto : IDto
{
    public string EmailOrUsername { get; set; }
    public string Password { get; set; }
    public string? AuthenticatorCode { get; set; }
}