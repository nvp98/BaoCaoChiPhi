using BaoCaoChiPhi.Application.DTOs;
using BaoCaoChiPhi.Application.Features.BienBanGiaoNhan.Queries;
using BaoCaoChiPhi.Application.Interfaces;
using BaoCaoChiPhi.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BaoCaoChiPhi.Infrastructure.Repositories;

public class BienBanGiaoNhanRepository(ProductDataDbContext context) : IBienBanGiaoNhanRepository
{
    public async Task<(IReadOnlyList<BienBanGiaoNhanDto> Data, int TotalRecords)> GetListAsync(
        GetBienBanListQuery request,
        CancellationToken cancellationToken = default)
    {
        var query = context.TblReportBienBanGiaoNhan.AsQueryable();

        // --- Filters ---
        if (request.FromDate.HasValue)
            query = query.Where(x => x.NgayTao.Date >= request.FromDate.Value.Date);

        if (request.ToDate.HasValue)
            query = query.Where(x => x.NgayTao.Date <= request.ToDate.Value.Date);

        if (request.Shift.HasValue)
        {
            var shiftStr = request.Shift.Value.ToString();
            query = query.Where(x => x.Ca == shiftStr);
        }

        if (!string.IsNullOrWhiteSpace(request.MaterialCode))
            query = query.Where(x => x.MaVatTu_Sap == request.MaterialCode);

        if (!string.IsNullOrWhiteSpace(request.WorkshopFrom))
            query = query.Where(x => x.TenXuong_BG == request.WorkshopFrom);

        if (!string.IsNullOrWhiteSpace(request.WorkshopTo))
            query = query.Where(x => x.TenXuong_BN == request.WorkshopTo);

        if (!string.IsNullOrWhiteSpace(request.PlantFrom))
            query = query.Where(x => x.TenPhongBan_BG == request.PlantFrom);

        if (!string.IsNullOrWhiteSpace(request.PlantTo))
            query = query.Where(x => x.TenPhongBan_BN == request.PlantTo);

        // --- GroupBy ---
        var grouped = query.GroupBy(x => new
        {
            ProductionDate = x.NgayTao.Date,
            x.Ca,
            x.ID_VatTu,
            x.ID_PhongBan_BG,
            x.ID_Xuong_BG,
            x.ID_PhongBan_BN,
            x.ID_Xuong_BN
        });

        var total = await grouped.CountAsync(cancellationToken);

        // Project server-side (không bao gồm ShiftName — map client-side)
        var raw = await grouped
            .Select(g => new
            {
                g.Key.ProductionDate,
                Ca = g.Key.Ca,
                MaterialName = g.Max(x => x.TenVatTu_Sap) ?? g.Max(x => x.TenVatTu),
                MaterialCode = g.Max(x => x.MaVatTu_Sap),
                WorkshopFrom = g.Max(x => x.TenXuong_BG),
                WorkshopTo = g.Max(x => x.TenXuong_BN),
                PlantFrom = g.Max(x => x.TenPhongBan_BG),
                PlantTo = g.Max(x => x.TenPhongBan_BN),
                Weight = g.Sum(x => x.KL_QuyKho_BN ?? 0),
                Unit = g.Max(x => x.DonViTinh)
            })
            .OrderByDescending(x => x.ProductionDate)
            .ThenBy(x => x.Ca)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        // --- Client-side mapping (parse Ca string → int, gán ShiftName) ---
        var items = raw.Select(r =>
        {
            var caInt = int.TryParse(r.Ca, out var parsed) ? parsed : 0;
            return new BienBanGiaoNhanDto
            {
                ProductionDate = r.ProductionDate,
                Shift = caInt,
                ShiftName = caInt switch { 1 => "Ngày", 2 => "Đêm", _ => r.Ca ?? string.Empty },
                MaterialName = r.MaterialName ?? string.Empty,
                MaterialCode = r.MaterialCode ?? string.Empty,
                WorkshopFrom = r.WorkshopFrom ?? string.Empty,
                WorkshopTo = r.WorkshopTo ?? string.Empty,
                PlantFrom = r.PlantFrom ?? string.Empty,
                PlantTo = r.PlantTo ?? string.Empty,
                Weight = r.Weight,
                Unit = r.Unit ?? string.Empty
            };
        }).ToList();

        return (items, total);
    }
}
