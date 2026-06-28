using BaoCaoChiPhi.Application.Common;
using BaoCaoChiPhi.Application.DTOs;
using BaoCaoChiPhi.Domain.Interfaces;
using MediatR;

namespace BaoCaoChiPhi.Application.Features.BaoCaoChiPhis.Queries.GetBaoCaoById;

public class GetBaoCaoByIdQueryHandler(IBaoCaoChiPhiRepository repository)
    : IRequestHandler<GetBaoCaoByIdQuery, Result<BaoCaoChiPhiDto>>
{
    public async Task<Result<BaoCaoChiPhiDto>> Handle(GetBaoCaoByIdQuery request, CancellationToken cancellationToken)
    {
        var baoCao = await repository.GetWithChiTietsAsync(request.Id, cancellationToken);

        if (baoCao is null)
            return Result<BaoCaoChiPhiDto>.Failure($"Không tìm thấy báo cáo với Id: {request.Id}");

        var dto = new BaoCaoChiPhiDto(
            baoCao.Id,
            baoCao.TieuDe,
            baoCao.MoTa,
            baoCao.TongTien,
            baoCao.NgayLap,
            baoCao.TrangThai,
            baoCao.NguoiLap,
            baoCao.CreatedAt,
            baoCao.ChiTiets.Select(ct => new ChiTietChiPhiDto(
                ct.Id, ct.DienGiai, ct.DanhMuc, ct.SoLuong, ct.DonGia, ct.ThanhTien
            )).ToList()
        );

        return Result<BaoCaoChiPhiDto>.Success(dto);
    }
}
