using BaoCaoChiPhi.Application.Common;
using BaoCaoChiPhi.Application.DTOs;
using MediatR;

namespace BaoCaoChiPhi.Application.Features.BaoCaoChiPhis.Queries.GetBaoCaoById;

public record GetBaoCaoByIdQuery(Guid Id) : IRequest<Result<BaoCaoChiPhiDto>>;
