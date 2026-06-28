using BaoCaoChiPhi.Application.DTOs;
using BaoCaoChiPhi.Application.Features.BienBanGiaoNhan.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BaoCaoChiPhi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BienBanGiaoNhanController(IMediator mediator) : ControllerBase
{
    /// <summary>Lấy danh sách biên bản giao nhận vật liệu (có phân trang)</summary>
    [HttpGet]
    [ProducesResponseType(typeof(PagedApiResponse<BienBanGiaoNhanDto>), 200)]
    public async Task<IActionResult> GetList([FromQuery] GetBienBanListQuery query, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(query, cancellationToken);
        return Ok(result);
    }
}
