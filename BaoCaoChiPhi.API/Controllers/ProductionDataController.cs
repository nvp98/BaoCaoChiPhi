using BaoCaoChiPhi.Application.DTOs;
using BaoCaoChiPhi.Application.Features.ProductionData.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BaoCaoChiPhi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProductionDataController(IMediator mediator) : ControllerBase
{
    /// <summary>Lấy danh sách dữ liệu sản xuất từ ChiPhi_ProductionData (có phân trang)</summary>
    [HttpGet]
    [ProducesResponseType(typeof(PagedApiResponse<ProductionDataDto>), 200)]
    public async Task<IActionResult> GetList([FromQuery] GetProductionDataQuery query, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(query, cancellationToken);
        return Ok(result);
    }
}
