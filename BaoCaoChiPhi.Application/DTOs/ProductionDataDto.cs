namespace BaoCaoChiPhi.Application.DTOs;

public class ProductionDataDto
{
    public DateOnly? ProductionDate { get; set; }
    public string Shift { get; set; } = string.Empty;
    public string ShiftName { get; set; } = string.Empty;
    public int? CostType { get; set; }
    public string WorkCenter { get; set; } = string.Empty;
    public string MaterialName { get; set; } = string.Empty;
    public string MaterialCode { get; set; } = string.Empty;
    public decimal? Weight { get; set; }
    public string Unit { get; set; } = string.Empty;
}
