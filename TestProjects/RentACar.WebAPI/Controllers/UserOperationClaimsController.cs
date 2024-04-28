using MenCore.Application.Request;
using Microsoft.AspNetCore.Mvc;
using RentACar.Application.Features.UserOperationClaims.Commands.Create;
using RentACar.Application.Features.UserOperationClaims.Commands.Delete;
using RentACar.Application.Features.UserOperationClaims.Commands.Update;
using RentACar.Application.Features.UserOperationClaims.Queries.GetById;
using RentACar.Application.Features.UserOperationClaims.Queries.GetByOperationClaimId;
using RentACar.Application.Features.UserOperationClaims.Queries.GetByUserId;
using RentACar.Application.Features.UserOperationClaims.Queries.GetList;

namespace RentACar.WebAPI.Controllers;

[Route("api/[controller]")]
public class UserOperationClaimsController : BaseController
{
    [HttpGet("GetListIncludable")]
    public async Task<IActionResult> GetListWithIncludableTableAsync([FromQuery] PageRequest pageRequest)
    {
        GetListUserOperationClaimQuery query = new() { PageRequest = pageRequest };
        var response = await Mediator.Send(query);
        return Ok(response);
    }

    [HttpGet("GetById/{id}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
    {
        GetByIdUserOperationClaimQuery query = new() { Id = id };
        var response = await Mediator.Send(query);
        return Ok(response);
    }

    [HttpGet("GetByUserId/{userId}")]
    public async Task<IActionResult> GetByUserIdAsync([FromRoute] int userId)
    {
        GetByUserIdUserOperationClaimQuery query = new() { UserId = userId };
        var response = await Mediator.Send(query);
        return Ok(response);
    }

    [HttpGet("GetByOperationClaimId/{operationClaimId}")]
    public async Task<IActionResult> GetByOperationClaimIdAsync([FromRoute] int operationClaimId)
    {
        GetByOperationClaimIdUserOperationClaimQuery query = new() { OperationClaimId = operationClaimId };
        GetByOperationClaimIdUserOperationClaimResponse response = await Mediator.Send(query);
        return Ok(response);
    }

    [HttpPost("Create")]
    public async Task<IActionResult> CreateAsync([FromBody] CreatedUserOperationClaimCommand command)
    {
        CreateUserOperationClaimResponse response = await Mediator.Send(command);
        return Ok(response);
    }

    [HttpPut("Update")]
    public async Task<IActionResult> UpdateAsync([FromBody] UpdatedUserOperationClaimCommand command)
    {
        UpdateUserOperationClaimResponse response = await Mediator.Send(command);
        return Ok(response);
    }

    [HttpDelete("Delete")]
    public async Task<IActionResult> DeleteAsync([FromBody] DeletedUserOperationClaimCommand command)
    {
        DeleteUserOperationClaimResponse response = await Mediator.Send(command);
        return Ok(response);
    }
}