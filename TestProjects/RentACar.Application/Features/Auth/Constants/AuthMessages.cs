namespace RentACar.Application.Features.Auth.Constants;

public class AuthMessages
{
    public static readonly string? EmailAuthenticatorDontExists = "Email authenticator don't exists";
    public static readonly string? OtpAuthenticatorDontExists = "Otp authenticator don't exists";
    public static readonly string? AlreadyVerifiedOtpAuthenticatorIsExits = "Already verified otp authenticator is exists";
    public static readonly string? EmailActivationKeyDontExits = "Email Activation Key don't exists";
    public static readonly string? InvalidRefreshToken = "Invalid refresh token";

    public static readonly string? UserDontExists = "User don't exists";
    public static readonly string? PasswordDontMatch = "Password don't exists";
    public static readonly string? UserMailAlreadyExists = "User mail alreadry exists";
}