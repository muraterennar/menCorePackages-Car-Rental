using MenCore.Application.Dtos;
using MenCore.Security.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using RentACar.Application.Features.Auth.Commands.EnableEmailAuthenticator;
using RentACar.Application.Features.Auth.Commands.EnableOtpAuthenticator;
using RentACar.Application.Features.Auth.Commands.Login;
using RentACar.Application.Features.Auth.Commands.RefleshToken;
using RentACar.Application.Features.Auth.Commands.Register;
using RentACar.Application.Features.Auth.Commands.RevokeToken;
using RentACar.Application.Features.Auth.Commands.VerifyEmailAuthenticator;
using RentACar.Application.Features.Auth.Commands.VerifyOtpAuthenticator;

namespace RentACar.WebAPI.Controllers;

[Route("api/[controller]")]
public class AuthController : BaseController
{
    private readonly WebAPIConfiguration _configuration;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        _configuration = configuration.GetSection("WebAPIConfiguration").Get<WebAPIConfiguration>();
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] UserForLoginDto userForLoginDto)
    {
        LoginCommand? loginCommand = new() { UserForLoginDto = userForLoginDto, IPAddress = getIpAddress() };
        var response = await Mediator.Send(loginCommand);

        if (response.RefreshToken is not null)
            setRefreshTokenToCookie(response.RefreshToken);

        return Ok(response.ToHttpResponse());
    }

    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] UserForRegisterDto userForRegisterDto)
    {
        RegisterCommand registerCommand = new() { UserForRegisterDto = userForRegisterDto, IPAddress = getIpAddress() };
        var result = await Mediator.Send(registerCommand);
        setRefreshTokenToCookie(result.RefreshToken);
        return Created("", result.AccessToken);
    }

    [HttpGet("RefreshToken")]
    public async Task<IActionResult> RefreshToken()
    {
        RefreshTokenCommand refreshTokenCommand = new()
            { RefleshToken = getRefreshTokenFromCookies(), IPAddress = getIpAddress() };
        var result = await Mediator.Send(refreshTokenCommand);
        setRefreshTokenToCookie(result.RefreshToken);
        return Created("", result.AccessToken);
    }

    [HttpPut("RevokeToken")]
    public async Task<IActionResult> RevokeToken(
        [FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Allow)]
        string? refreshToken)
    {
        RevokeTokenCommand revokeTokenCommand = new()
            { Token = refreshToken ?? getRefreshTokenFromCookies(), IPAddress = getIpAddress() };
        var result = await Mediator.Send(revokeTokenCommand);
        return Ok(result);
    }

    [HttpGet("EnableEmailAuthenticator")]
    public async Task<IActionResult> EnableEmailAuthenticator()
    {
        EnableEmailAuthenticatorCommand enableEmailAuthenticatorCommand =
            new()
            {
                UserId = getUserIdFromRequest(_httpContextAccessor),
                VerifyEmailUrlPrefix = $"{_configuration.APIDomain}/Auth/VerifyEmailAuthenticator"
            };
        await Mediator.Send(enableEmailAuthenticatorCommand);

        return Ok();
    }

    [HttpGet("EnableOtpAuthenticator")]
    public async Task<IActionResult> EnableOtpAuthenticator()
    {
        EnableOtpAuthenticatorCommand enableOtpAuthenticatorCommand =
            new() { UserId = getUserIdFromRequest(_httpContextAccessor) };
        var result = await Mediator.Send(enableOtpAuthenticatorCommand);

        return Ok(result);
    }


    [HttpGet("VerifyEmailAuthenticator")]
    public async Task<IActionResult> VerifyEmailAuthenticator(
        [FromQuery] VerifyEmailAuthenticatorCommand verifyEmailAuthenticatorCommand)
    {
        VerifyEmailAuthenticatorResponse message = await Mediator.Send(verifyEmailAuthenticatorCommand);
        return Ok(message);
    }

    [HttpPost("VerifyOtpAuthenticator")]
    public async Task<IActionResult> VerifyOtpAuthenticator([FromBody] string authenticatorCode)
    {
        VerifyOtpAuthenticatorCommand verifyEmailAuthenticatorCommand =
            new() { UserId = getUserIdFromRequest(_httpContextAccessor), ActivationCode = authenticatorCode };

        VerifyOtpAuthenticatorResponse message = await Mediator.Send(verifyEmailAuthenticatorCommand);
        return Ok(message);
    }

    private string? getRefreshTokenFromCookies()
    {
        return Request.Cookies["refreshToken"];
    }

    private void setRefreshTokenToCookie(RefreshToken refreshToken)
    {
        CookieOptions cookieOptions = new() { HttpOnly = true, Expires = DateTime.UtcNow.AddDays(7) };
        Response.Cookies.Append("refreshToken", refreshToken.Token, cookieOptions);
    }
}