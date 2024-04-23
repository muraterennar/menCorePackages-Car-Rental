using Microsoft.AspNetCore.Mvc;
using RentACar.Application.Features.Users.Commands.Create;
using RentACar.Application.Features.Users.Commands.Delete;
using RentACar.Application.Features.Users.Commands.Update;

namespace RentACar.WebAPI.Controllers;

[Route("api/[controller]")]
public class UsersController : BaseController
{
    [HttpPost("add")]
    public async Task<IActionResult> AddUser([FromBody] CreatedUserCommand createdUserCommand)
    {
        var response = await Mediator.Send(createdUserCommand);
        return Ok(response);
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateUser([FromBody] UpdatedUserCommand updatedUserCommand)
    {
        var response = await Mediator.Send(updatedUserCommand);
        return Ok(response);
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteUser([FromBody] DeletedUserCommand deletedUserCommand)
    {
        var response = await Mediator.Send(deletedUserCommand);
        return Ok(response);
    }
}