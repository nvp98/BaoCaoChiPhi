using BaoCaoChiPhi.Domain.Common;

namespace BaoCaoChiPhi.Domain.Entities;

public class ChiTietChiPhi : BaseEntity
{
    public Guid BaoCaoId { get; private set; }
    public string DienGiai { get; private set; } = string.Empty;
    public string? DanhMuc { get; private set; }
    public decimal SoLuong { get; private set; }
    public decimal DonGia { get; private set; }
    public decimal ThanhTien => SoLuong * DonGia;

    private ChiTietChiPhi() { }

    public static ChiTietChiPhi Create(Guid baoCaoId, string dienGiai, decimal soLuong, decimal donGia, string? danhMuc = null)
    {
        return new ChiTietChiPhi
        {
            BaoCaoId = baoCaoId,
            DienGiai = dienGiai,
            SoLuong = soLuong,
            DonGia = donGia,
            DanhMuc = danhMuc
        };
    }
}
