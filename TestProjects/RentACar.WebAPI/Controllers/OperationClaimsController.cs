using MenCore.Application.Request;
using Microsoft.AspNetCore.Mvc;
using RentACar.Application.Features.OperationClaims.Commands.Create;
using RentACar.Application.Features.OperationClaims.Commands.Delete;
using RentACar.Application.Features.OperationClaims.Commands.Update;
using RentACar.Application.Features.OperationClaims.Queries.GetAll;
using RentACar.Application.Features.OperationClaims.Queries.GetById;
using RentACar.Application.Features.OperationClaims.Queries.GetByName;

namespace RentACar.WebAPI.Controllers;

[Route("api/[controller]")]
public class OperationClaimsController : BaseController
{
    [HttpGet("GetList")]
    public async Task<IActionResult> GetList([FromQuery] PageRequest pageRequest)
    {
        GetAllOperationClaimQuery? getAllOperationClaimQuery = new() { PageRequest = pageRequest };
        var response = await Mediator.Send(getAllOperationClaimQuery);
        return Ok(response);
    }

    [HttpGet("GetById/{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        GetByIdOperationClaimQuery? getByIdOperationClaimQuery = new() { Id = id };
        var response = await Mediator.Send(getByIdOperationClaimQuery);
        return Ok(response);
    }

    [HttpGet("GetByName/{name}")]
    public async Task<IActionResult> GetByName([FromRoute] string name)
    {
        GetByNameOperationClaimQuery? getByNameOperationClaimQuery = new() { Name = name };
        var response = await Mediator.Send(getByNameOperationClaimQuery);
        return Ok(response);
    }

    [HttpPost("Add")]
    public async Task<IActionResult> Add([FromBody] CreateOperationClaimCommands createOperationClaimCommands)
    {
        var response = await Mediator.Send(createOperationClaimCommands);
        return Ok(response);
    }

    [HttpPut("Update")]
    public async Task<IActionResult> Update([FromBody] UpdateOperationClaimCommands updateOperationClaimCommands)
    {
        var response = await Mediator.Send(updateOperationClaimCommands);
        return Ok(response);
    }

    [HttpDelete("Delete")]
    public async Task<IActionResult> Delete([FromBody] DeleteOperationClaimCommands deleteOperationClaimCommands)
    {
        var response = await Mediator.Send(deleteOperationClaimCommands);
        return Ok(response);
    }
}