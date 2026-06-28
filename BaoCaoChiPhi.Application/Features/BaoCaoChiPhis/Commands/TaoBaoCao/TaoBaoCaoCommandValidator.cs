using FluentValidation;

namespace BaoCaoChiPhi.Application.Features.BaoCaoChiPhis.Commands.TaoBaoCao;

public class TaoBaoCaoCommandValidator : AbstractValidator<TaoBaoCaoCommand>
{
    public TaoBaoCaoCommandValidator()
    {
        RuleFor(x => x.TieuDe)
            .NotEmpty().WithMessage("Tiêu đề không được để trống")
            .MaximumLength(500).WithMessage("Tiêu đề không quá 500 ký tự");

        RuleFor(x => x.NguoiLap)
            .NotEmpty().WithMessage("Người lập không được để trống")
            .MaximumLength(200).WithMessage("Người lập không quá 200 ký tự");

        RuleFor(x => x.NgayLap)
            .NotEmpty().WithMessage("Ngày lập không được để trống")
            .LessThanOrEqualTo(DateTime.Today).WithMessage("Ngày lập không được là ngày tương lai");
    }
}
