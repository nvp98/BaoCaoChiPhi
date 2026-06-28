using BaoCaoChiPhi.Application.Common;
using BaoCaoChiPhi.Domain.Entities;
using BaoCaoChiPhi.Domain.Interfaces;
using MediatR;

namespace BaoCaoChiPhi.Application.Features.BaoCaoChiPhis.Commands.TaoBaoCao;

public class TaoBaoCaoCommandHandler(
    IBaoCaoChiPhiRepository repository,
    IUnitOfWork unitOfWork
) : IRequestHandler<TaoBaoCaoCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(TaoBaoCaoCommand request, CancellationToken cancellationToken)
    {
        var baoCao = BaoCaoChiPhiEntity.Create(
            request.TieuDe,
            request.NguoiLap,
            request.NgayLap,
            request.MoTa);

        await repository.AddAsync(baoCao, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(baoCao.Id);
    }
}
