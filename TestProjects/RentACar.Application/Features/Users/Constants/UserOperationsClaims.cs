using MenCore.Security.Constants;

namespace RentACar.Application.Features.Users.Constants;

public abstract class UserOperationsClaims : GeneralOperationClaims
{
    public const string UserAdmin = "users.admin";

    public const string UserRead = "users.read";
    public const string UserWrite = "users.write";
}