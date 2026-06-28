using BaoCaoChiPhi.Application.Common;
using MediatR;

namespace BaoCaoChiPhi.Application.Features.BaoCaoChiPhis.Commands.TaoBaoCao;

public record TaoBaoCaoCommand(
    string TieuDe,
    string NguoiLap,
    DateTime NgayLap,
    string? MoTa = null
) : IRequest<Result<Guid>>;
