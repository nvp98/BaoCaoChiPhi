using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BaoCaoChiPhi.Infrastructure.Persistence.Entities;

[Table("ChiPhi_ProductionData", Schema = "dbo")]
public class ChiPhiProductionData
{
    [Key]
    public long Id { get; set; }
    public string? CongDoan { get; set; }
    public string? MaChiPhi { get; set; }
    public DateOnly? Ngay { get; set; }
    public byte? Ca { get; set; }
    public string? Kip { get; set; }
    public string? MaVatTu { get; set; }
    public string? TenVatTu { get; set; }
    public decimal? KhoiLuong { get; set; }
    public string? DonViTinh { get; set; }
    public DateTime? CreatedDate { get; set; }
}
