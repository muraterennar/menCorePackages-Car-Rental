using MediatR;
using MenCore.Security.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace RentACar.WebAPI.Controllers;

public class BaseController : ControllerBase
{
    private IMediator? _mediator;
    protected IMediator? Mediator => _mediator ??= HttpContext?.RequestServices?.GetService<IMediator>();
    
    protected string? getIpAddress()
    {
        if (Request.Headers.ContainsKey("X-Forwarded-For"))
            return Request.Headers["X-Forwarded-For"];
        return HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString();
    }

    protected int getUserIdFromRequest(IHttpContextAccessor httpContextAccessor)
    {
        var userId = httpContextAccessor.HttpContext.User.GetUserId();
        return userId;
    }
}