using BaoCaoChiPhi.Application.DTOs;
using BaoCaoChiPhi.Application.Interfaces;
using MediatR;

namespace BaoCaoChiPhi.Application.Features.ProductionData.Queries;

public class GetProductionDataQueryHandler(IProductionDataRepository repository)
    : IRequestHandler<GetProductionDataQuery, PagedApiResponse<ProductionDataDto>>
{
    public async Task<PagedApiResponse<ProductionDataDto>> Handle(
        GetProductionDataQuery request,
        CancellationToken cancellationToken)
    {
        var (data, total) = await repository.GetListAsync(request, cancellationToken);

        return PagedApiResponse<ProductionDataDto>.Success(
            data.ToList(),
            total,
            request.PageNumber,
            request.PageSize);
    }
}
