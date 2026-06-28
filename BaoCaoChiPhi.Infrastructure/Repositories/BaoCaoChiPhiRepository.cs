using BaoCaoChiPhi.Domain.Entities;
using BaoCaoChiPhi.Domain.Enums;
using BaoCaoChiPhi.Domain.Interfaces;
using BaoCaoChiPhi.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BaoCaoChiPhi.Infrastructure.Repositories;

public class BaoCaoChiPhiRepository(AppDbContext context)
    : BaseRepository<BaoCaoChiPhiEntity>(context), IBaoCaoChiPhiRepository
{
    public async Task<IReadOnlyList<BaoCaoChiPhiEntity>> GetByNguoiLapAsync(string nguoiLap, CancellationToken cancellationToken = default)
        => await DbSet.Where(x => x.NguoiLap == nguoiLap).ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<BaoCaoChiPhiEntity>> GetByTrangThaiAsync(TrangThaiChiPhi trangThai, CancellationToken cancellationToken = default)
        => await DbSet.Where(x => x.TrangThai == trangThai).ToListAsync(cancellationToken);

    public async Task<BaoCaoChiPhiEntity?> GetWithChiTietsAsync(Guid id, CancellationToken cancellationToken = default)
        => await DbSet.Include(x => x.ChiTiets).FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
}
