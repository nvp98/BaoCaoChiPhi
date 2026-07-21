using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BaoCaoChiPhi.Infrastructure.Persistence.Entities;

[Table("Tbl_Report_BienBanGiaoNhan", Schema = "dbo")]
public class TblReportBienBanGiaoNhan
{
    [Key]
    public long ID_CT_BBGN { get; set; }
    public long? ID_BBGN { get; set; }
    public string? SoPhieu { get; set; }
    public DateTime NgayTao { get; set; }
    public string? Kip { get; set; }
    public string? Ca { get; set; }

    // Vật tư
    public long? ID_VatTu { get; set; }
    public string? TenVatTu { get; set; }
    public string? MaVatTu_Sap { get; set; }
    public string? TenVatTu_Sap { get; set; }
    public string? DonViTinh { get; set; }

    // Bên giao
    public long? ID_PhongBan_BG { get; set; }
    public DateTime ThoiGianXuLyBG { get; set; }
    public string? TenPhongBan_BG { get; set; }
    public string? MaPhongBan_BG { get; set; }
    public long? ID_Xuong_BG { get; set; }
    public string? TenXuong_BG { get; set; }

    // Bên nhận
    public long? ID_PhongBan_BN { get; set; }
    public DateTime? ThoiGianXuLyBN { get; set; }
    public string? TenPhongBan_BN { get; set; }
    public string? MaPhongBan_BN { get; set; }
    public long? ID_Xuong_BN { get; set; }
    public string? TenXuong_BN { get; set; }

    // Khối lượng
    public decimal? KhoiLuong_BG { get; set; }
    public decimal? KL_QuyKho_BG { get; set; }
    public decimal? KhoiLuong_BN { get; set; }
    public decimal? KL_QuyKho_BN { get; set; }

    public string? GhiChu { get; set; }
    public DateTime? LastSync { get; set; }
}
