namespace BaoCaoChiPhi.Application.DTOs;

public class ProductionDataDto
{
    public long Id { get; set; }
    public string CongDoan { get; set; } = string.Empty;
    public string MaChiPhi { get; set; } = string.Empty;
    public DateOnly? Ngay { get; set; }
    public byte? Ca { get; set; }
    public string Kip { get; set; } = string.Empty;
    public string MaVatTu { get; set; } = string.Empty;
    public string TenVatTu { get; set; } = string.Empty;
    public decimal? KhoiLuong { get; set; }
    public string DonViTinh { get; set; } = string.Empty;
    public DateTime? CreatedDate { get; set; }
}
