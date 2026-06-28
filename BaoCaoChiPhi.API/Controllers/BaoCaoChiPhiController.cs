using BaoCaoChiPhi.Application.Features.BaoCaoChiPhis.Commands.TaoBaoCao;
using BaoCaoChiPhi.Application.Features.BaoCaoChiPhis.Queries.GetBaoCaoById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BaoCaoChiPhi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BaoCaoChiPhiController(IMediator mediator) : ControllerBase
{
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetBaoCaoByIdQuery(id), cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
    }

    [HttpPost]
    public async Task<IActionResult> TaoBaoCao([FromBody] TaoBaoCaoCommand command, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);
        return result.IsSuccess
            ? CreatedAtAction(nameof(GetById), new { id = result.Value }, result.Value)
            : BadRequest(result.Error);
    }
}
