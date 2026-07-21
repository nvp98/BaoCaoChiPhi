using BaoCaoChiPhi.Application.DTOs;
using BaoCaoChiPhi.Application.Features.ProductionData.Queries;

namespace BaoCaoChiPhi.Application.Interfaces;

public interface IProductionDataRepository
{
    Task<(IReadOnlyList<ProductionDataDto> Data, int TotalRecords)> GetListAsync(
        GetProductionDataQuery query,
        CancellationToken cancellationToken = default);
}
