using Core.Security.Entities;

namespace Core.Security.JWT;

public interface ITokenHelper
{
    // Bu yöntem, kullanıcıya ve yetkilendirme taleplerine dayanarak bir erişim belirteci oluşturur.
    AccessToken CreateToken(User user, IList<OperationClaim> operationClaims);

    // Bu yöntem, kullanıcıya dayalı olarak ve bir IP adresi ile birlikte bir yenileme belirteci oluşturur.
    RefreshToken CreateRefreshToken(User user, string ipAddress);
}
