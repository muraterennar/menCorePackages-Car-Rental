using MenCore.Application.Request;
using MenCore.Application.Responses;
using MenCore.Persistence.Dynamic;
using Microsoft.AspNetCore.Mvc;
using RentACar.Application.Features.Models.Queries.GetList;
using RentACar.Application.Features.Models.Queries.GetListByDynamic;

namespace RentACar.WebAPI.Controllers;

public class ModelsController : BaseController
{
    [HttpGet("GetList")]
    public async Task<IActionResult> GetList ([FromQuery] PageRequest pageRequest)
    {
        GetListModelQuery getListModelQuery = new() { PageRequest = pageRequest };
        GetListResponse<GetListModelListItemDto> response = await Mediator.Send(getListModelQuery);
        return Ok(response);
    }

    [HttpPost("GetList/ByDynamic")]
    public async Task<IActionResult> GetListByDynamic ([FromQuery] PageRequest pageRequest, [FromBody] DynamicQuery dynamicQuery)
    {
        //TODO: Arama yöntemi İlişkili Tablolarda "Brand.brandName" veya "Fuel.FuelName" şeklinde olmalıdır

        GetListByDynamicModelQuery getListByDynamicModelQuery = new() { PageRequest = pageRequest, DynamicQuery = dynamicQuery };
        GetListResponse<GetListByDynamicModelListItemDto> response = await Mediator.Send(getListByDynamicModelQuery);
        return Ok(response);
    }
}