using MenCore.Security.Constants;

namespace RentACar.Application.Features.UserOperationClaims.Constants;

public abstract class UserOperationClaimsOperationClaims : GeneralOperationClaims
{
    public const string UserOperationAdmin = "useroperationclaims.admin";

    public const string UserOperationRead = "useroperationclaims.read";
    public const string UserOperationWrite = "useroperationclaims.write";
}