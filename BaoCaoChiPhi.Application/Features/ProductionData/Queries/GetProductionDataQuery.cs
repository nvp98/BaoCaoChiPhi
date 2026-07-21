using BaoCaoChiPhi.Application.DTOs;
using MediatR;

namespace BaoCaoChiPhi.Application.Features.ProductionData.Queries;

public record GetProductionDataQuery(
    string? CongDoan,
    string? MaChiPhi,
    DateOnly? FromDate,
    DateOnly? ToDate,
    byte? Ca,
    string? Kip,
    string? MaVatTu,
    string? TenVatTu,
    string? DonViTinh,
    // DateTime? FromCreatedDate,
    // DateTime? ToCreatedDate,
    int PageNumber = 1,
    int PageSize = 20
) : IRequest<PagedApiResponse<ProductionDataDto>>;
