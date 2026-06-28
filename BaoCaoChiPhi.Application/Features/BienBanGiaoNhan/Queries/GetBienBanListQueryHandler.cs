using BaoCaoChiPhi.Application.DTOs;
using BaoCaoChiPhi.Application.Interfaces;
using MediatR;

namespace BaoCaoChiPhi.Application.Features.BienBanGiaoNhan.Queries;

public class GetBienBanListQueryHandler(IBienBanGiaoNhanRepository repository)
    : IRequestHandler<GetBienBanListQuery, PagedApiResponse<BienBanGiaoNhanDto>>
{
    public async Task<PagedApiResponse<BienBanGiaoNhanDto>> Handle(
        GetBienBanListQuery request,
        CancellationToken cancellationToken)
    {
        var (data, total) = await repository.GetListAsync(request, cancellationToken);

        return PagedApiResponse<BienBanGiaoNhanDto>.Success(
            data.ToList(),
            total,
            request.PageNumber,
            request.PageSize);
    }
}
