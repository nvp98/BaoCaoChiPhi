using BaoCaoChiPhi.Application.DTOs;
using MediatR;

namespace BaoCaoChiPhi.Application.Features.ProductionData.Queries;

public record GetProductionDataQuery(
    string? WorkCenter,
    int? CostType,
    DateOnly? FromDate,
    DateOnly? ToDate,
    byte? Shift,
    string? ShiftName,
    string? MaterialCode,
    string? MaterialName,
    string? Unit,
    int PageNumber = 1,
    int PageSize = 20
) : IRequest<PagedApiResponse<ProductionDataDto>>;
