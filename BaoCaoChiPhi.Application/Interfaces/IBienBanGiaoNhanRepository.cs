using BaoCaoChiPhi.Application.DTOs;
using BaoCaoChiPhi.Application.Features.BienBanGiaoNhan.Queries;

namespace BaoCaoChiPhi.Application.Interfaces;

public interface IBienBanGiaoNhanRepository
{
    Task<(IReadOnlyList<BienBanGiaoNhanDto> Data, int TotalRecords)> GetListAsync(
        GetBienBanListQuery query,
        CancellationToken cancellationToken = default);
}
