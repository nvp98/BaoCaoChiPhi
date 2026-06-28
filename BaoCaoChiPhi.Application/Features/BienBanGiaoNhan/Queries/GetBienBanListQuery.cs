using BaoCaoChiPhi.Application.DTOs;
using MediatR;

namespace BaoCaoChiPhi.Application.Features.BienBanGiaoNhan.Queries;

public record GetBienBanListQuery(
    DateTime? FromDate,
    DateTime? ToDate,
    int? Shift,
    string? MaterialCode,
    string? WorkshopFrom,
    string? WorkshopTo,
    string? PlantFrom,
    string? PlantTo,
    int PageNumber = 1,
    int PageSize = 20
) : IRequest<PagedApiResponse<BienBanGiaoNhanDto>>;
