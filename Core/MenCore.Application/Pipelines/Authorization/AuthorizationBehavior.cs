using MediatR;
using MenCore.CrossCuttingConserns.Exceptions.Types;
using MenCore.Security.Constants;
using MenCore.Security.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

namespace MenCore.Application.Pipelines.Authorization;

public class AuthorizationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>, ISecuredRequest
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthorizationBehavior(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    // İşlenen isteği denetler ve yetkilendirme sağlar.
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        // Kullanıcının rol taleplerini alır.
        var userRoleClaims = _httpContextAccessor.HttpContext.User.ClaimRoles();

        // Eğer kullanıcı rol talepleri yoksa yetkilendirme istisnası fırlatılır.
        if (userRoleClaims == null || userRoleClaims.Count == 0)
            throw new AuthorizationException("You are not authenticated.");

        // Kullanıcı taleplerinin, istek rolleri ile eşleşip eşleşmediğini kontrol eder.
        var isNotMatchAUserRoleClaimWithRequestRoles = userRoleClaims.FirstOrDefault(
            userRoleClaim => userRoleClaim == GeneralOperationClaims.Admin ||
                             request.Roles.Any(role => role == userRoleClaim)
        ).IsNullOrEmpty();

        // Eğer kullanıcının talepleri isteğin rolleri ile eşleşmiyorsa yetkilendirme istisnası fırlatılır.
        if (isNotMatchAUserRoleClaimWithRequestRoles)
            throw new AuthorizationException("You are not authorized");

        // Bir sonraki adımı (istek işleyicisini) çağırır ve işlenen yanıtı döndürür.
        var response = await next();

        return response;
    }
}