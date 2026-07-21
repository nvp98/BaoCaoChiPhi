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

        if (!string.IsNullOrWhiteSpace(request.CongDoan))
            query = query.Where(x => x.CongDoan == request.CongDoan);

        if (!string.IsNullOrWhiteSpace(request.MaChiPhi))
            query = query.Where(x => x.MaChiPhi == request.MaChiPhi);

        if (request.FromDate.HasValue)
            query = query.Where(x => x.Ngay >= request.FromDate.Value);

        if (request.ToDate.HasValue)
            query = query.Where(x => x.Ngay <= request.ToDate.Value);

        if (request.Ca.HasValue)
            query = query.Where(x => x.Ca == request.Ca.Value);

        if (!string.IsNullOrWhiteSpace(request.Kip))
            query = query.Where(x => x.Kip == request.Kip);

        if (!string.IsNullOrWhiteSpace(request.MaVatTu))
            query = query.Where(x => x.MaVatTu == request.MaVatTu);

        if (!string.IsNullOrWhiteSpace(request.TenVatTu))
            query = query.Where(x => x.TenVatTu != null && x.TenVatTu.Contains(request.TenVatTu));

        if (!string.IsNullOrWhiteSpace(request.DonViTinh))
            query = query.Where(x => x.DonViTinh == request.DonViTinh);

        // if (request.FromCreatedDate.HasValue)
        //     query = query.Where(x => x.CreatedDate >= request.FromCreatedDate.Value);

        // if (request.ToCreatedDate.HasValue)
        //     query = query.Where(x => x.CreatedDate <= request.ToCreatedDate.Value);

        var total = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(x => x.Ngay)
            .ThenBy(x => x.Ca)
            .ThenBy(x => x.CongDoan)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(x => new ProductionDataDto
            {
                Id = x.Id,
                CongDoan = x.CongDoan ?? string.Empty,
                MaChiPhi = x.MaChiPhi ?? string.Empty,
                Ngay = x.Ngay,
                Ca = x.Ca,
                Kip = x.Kip ?? string.Empty,
                MaVatTu = x.MaVatTu ?? string.Empty,
                TenVatTu = x.TenVatTu ?? string.Empty,
                KhoiLuong = x.KhoiLuong,
                DonViTinh = x.DonViTinh ?? string.Empty,
                CreatedDate = x.CreatedDate
            })
            .ToListAsync(cancellationToken);

        return (items, total);
    }
}
