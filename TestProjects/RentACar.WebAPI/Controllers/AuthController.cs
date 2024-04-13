using MediatR;
using Microsoft.AspNetCore.Mvc;
using RentACar.Application.Features.Auth.Commands.Login;

namespace RentACar.WebAPI.Controllers;

[Route("api/[controller]")]
public class AuthController : BaseController
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand loginCommand)
    {
        LoginResponse response = await Mediator.Send(loginCommand);
        return Ok(response);
    }
}