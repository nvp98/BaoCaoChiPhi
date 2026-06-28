namespace BaoCaoChiPhi.Application.DTOs;

public class BienBanGiaoNhanDto
{
    public DateTime ProductionDate { get; set; }
    public int Shift { get; set; }
    public string ShiftName { get; set; } = string.Empty;
    public string MaterialName { get; set; } = string.Empty;
    public string MaterialCode { get; set; } = string.Empty;
    public string WorkshopFrom { get; set; } = string.Empty;
    public string WorkshopTo { get; set; } = string.Empty;
    public string PlantFrom { get; set; } = string.Empty;
    public string PlantTo { get; set; } = string.Empty;
    public decimal Weight { get; set; }
    public string Unit { get; set; } = string.Empty;
}
