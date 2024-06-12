namespace RentACar.Application.Features.Auth.Commands.EnableOtpAuthenticator;

public class EnabledOtpAuthenticatorResponse
{
    public string? Otp { get; set; }
    public string? OtpQrCode { get; set; }
}