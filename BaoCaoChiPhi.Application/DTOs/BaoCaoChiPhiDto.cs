using BaoCaoChiPhi.Domain.Enums;

namespace BaoCaoChiPhi.Application.DTOs;

public record BaoCaoChiPhiDto(
    Guid Id,
    string TieuDe,
    string? MoTa,
    decimal TongTien,
    DateTime NgayLap,
    TrangThaiChiPhi TrangThai,
    string NguoiLap,
    DateTime CreatedAt,
    List<ChiTietChiPhiDto> ChiTiets
);

public record ChiTietChiPhiDto(
    Guid Id,
    string DienGiai,
    string? DanhMuc,
    decimal SoLuong,
    decimal DonGia,
    decimal ThanhTien
);
