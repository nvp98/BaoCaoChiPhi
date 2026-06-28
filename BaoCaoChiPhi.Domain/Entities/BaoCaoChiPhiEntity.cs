using BaoCaoChiPhi.Domain.Common;
using BaoCaoChiPhi.Domain.Enums;

namespace BaoCaoChiPhi.Domain.Entities;

public class BaoCaoChiPhiEntity : AuditableEntity
{
    public string TieuDe { get; private set; } = string.Empty;
    public string? MoTa { get; private set; }
    public decimal TongTien { get; private set; }
    public DateTime NgayLap { get; private set; }
    public TrangThaiChiPhi TrangThai { get; private set; } = TrangThaiChiPhi.ChoDuyet;
    public string NguoiLap { get; private set; } = string.Empty;

    private readonly List<ChiTietChiPhi> _chiTiets = [];
    public IReadOnlyCollection<ChiTietChiPhi> ChiTiets => _chiTiets.AsReadOnly();

    private BaoCaoChiPhiEntity() { }

    public static BaoCaoChiPhiEntity Create(string tieuDe, string nguoiLap, DateTime ngayLap, string? moTa = null)
    {
        return new BaoCaoChiPhiEntity
        {
            TieuDe = tieuDe,
            NguoiLap = nguoiLap,
            NgayLap = ngayLap,
            MoTa = moTa
        };
    }

    public void ThemChiTiet(ChiTietChiPhi chiTiet)
    {
        _chiTiets.Add(chiTiet);
        TinhLaiTongTien();
        SetUpdated();
    }

    public void XoaChiTiet(Guid chiTietId)
    {
        var item = _chiTiets.FirstOrDefault(x => x.Id == chiTietId);
        if (item != null)
        {
            _chiTiets.Remove(item);
            TinhLaiTongTien();
            SetUpdated();
        }
    }

    public void CapNhat(string tieuDe, string? moTa)
    {
        TieuDe = tieuDe;
        MoTa = moTa;
        SetUpdated();
    }

    public void Duyet(string nguoiDuyet)
    {
        TrangThai = TrangThaiChiPhi.DaDuyet;
        UpdatedBy = nguoiDuyet;
        SetUpdated();
    }

    public void TuChoi(string nguoiTuChoi)
    {
        TrangThai = TrangThaiChiPhi.TuChoi;
        UpdatedBy = nguoiTuChoi;
        SetUpdated();
    }

    private void TinhLaiTongTien() => TongTien = _chiTiets.Sum(x => x.ThanhTien);
}
