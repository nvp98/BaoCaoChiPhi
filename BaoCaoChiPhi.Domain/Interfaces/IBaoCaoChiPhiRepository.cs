using BaoCaoChiPhi.Domain.Entities;
using BaoCaoChiPhi.Domain.Enums;

namespace BaoCaoChiPhi.Domain.Interfaces;

public interface IBaoCaoChiPhiRepository : IRepository<BaoCaoChiPhiEntity>
{
    Task<IReadOnlyList<BaoCaoChiPhiEntity>> GetByNguoiLapAsync(string nguoiLap, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<BaoCaoChiPhiEntity>> GetByTrangThaiAsync(TrangThaiChiPhi trangThai, CancellationToken cancellationToken = default);
    Task<BaoCaoChiPhiEntity?> GetWithChiTietsAsync(Guid id, CancellationToken cancellationToken = default);
}
