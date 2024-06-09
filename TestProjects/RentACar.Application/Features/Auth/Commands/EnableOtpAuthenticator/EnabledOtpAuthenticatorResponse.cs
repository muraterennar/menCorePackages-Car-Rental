namespace RentACar.Application.Features.Auth.Commands.EnableOtpAuthenticator;

public class EnabledOtpAuthenticatorResponse
{
    public string SecretKey { get; set; }
    public string? Otp { get; set; }
}