using BaoCaoChiPhi.Application.DTOs;
using BaoCaoChiPhi.Application.Features.ProductionData.Queries;
using BaoCaoChiPhi.Application.Interfaces;
using BaoCaoChiPhi.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BaoCaoChiPhi.Infrastructure.Repositories;

public class ProductionDataRepository(ProductFormDbContext context) : IProductionDataRepository
{
    public async Task<(IReadOnlyList<ProductionDataDto> Data, int TotalRecords)> GetListAsync(
        GetProductionDataQuery request,
        CancellationToken cancellationToken = default)
    {
        var query = context.ChiPhiProductionData.AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.WorkCenter))
            query = query.Where(x => x.CongDoan == request.WorkCenter);

        if (request.CostType.HasValue)
            query = query.Where(x => x.MaChiPhi == request.CostType.Value.ToString());

        if (request.FromDate.HasValue)
            query = query.Where(x => x.Ngay >= request.FromDate.Value);

        if (request.ToDate.HasValue)
            query = query.Where(x => x.Ngay <= request.ToDate.Value);

        if (request.Shift.HasValue)
            query = query.Where(x => x.Ca == request.Shift.Value);

        if (!string.IsNullOrWhiteSpace(request.ShiftName))
            query = query.Where(x => x.Kip == request.ShiftName);

        if (!string.IsNullOrWhiteSpace(request.MaterialCode))
            query = query.Where(x => x.MaVatTu == request.MaterialCode);

        if (!string.IsNullOrWhiteSpace(request.MaterialName))
            query = query.Where(x => x.TenVatTu != null && x.TenVatTu.Contains(request.MaterialName));

        if (!string.IsNullOrWhiteSpace(request.Unit))
            query = query.Where(x => x.DonViTinh == request.Unit);

        // if (request.FromCreatedDate.HasValue)
        //     query = query.Where(x => x.CreatedDate >= request.FromCreatedDate.Value);

        // if (request.ToCreatedDate.HasValue)
        //     query = query.Where(x => x.CreatedDate <= request.ToCreatedDate.Value);

        var total = await query.CountAsync(cancellationToken);

        var rawItems = await query
            .OrderByDescending(x => x.Ngay)
            .ThenBy(x => x.Ca)
            .ThenBy(x => x.CongDoan)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(x => new
            {
                x.Ngay,
                x.Ca,
                x.Kip,
                x.MaChiPhi,
                x.CongDoan,
                x.TenVatTu,
                x.MaVatTu,
                x.KhoiLuong,
                x.DonViTinh
            })
            .ToListAsync(cancellationToken);

        var items = rawItems.Select(x => new ProductionDataDto
        {
            ProductionDate = x.Ngay,
            Shift = x.Ca?.ToString() ?? string.Empty,
            ShiftName = x.Kip ?? string.Empty,
            CostType = int.TryParse(x.MaChiPhi, out var costType) ? costType : null,
            WorkCenter = x.CongDoan ?? string.Empty,
            MaterialName = x.TenVatTu ?? string.Empty,
            MaterialCode = x.MaVatTu ?? string.Empty,
            Weight = x.KhoiLuong,
            Unit = x.DonViTinh ?? string.Empty
        }).ToList();

        return (items, total);
    }
}
