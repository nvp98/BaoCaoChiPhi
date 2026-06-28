using BaoCaoChiPhi.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace BaoCaoChiPhi.Infrastructure.Persistence;

public class ProductDataDbContext(DbContextOptions<ProductDataDbContext> options) : DbContext(options)
{
    public DbSet<TblReportBienBanGiaoNhan> TblReportBienBanGiaoNhan => Set<TblReportBienBanGiaoNhan>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TblReportBienBanGiaoNhan>(e =>
        {
            e.ToTable("Tbl_Report_BienBanGiaoNhan", "dbo");
            e.HasKey(x => x.ID_CT_BBGN);
            e.Property(x => x.KhoiLuong_BG).HasColumnType("decimal(18,4)");
            e.Property(x => x.KL_QuyKho_BG).HasColumnType("decimal(18,4)");
            e.Property(x => x.KhoiLuong_BN).HasColumnType("decimal(18,4)");
            e.Property(x => x.KL_QuyKho_BN).HasColumnType("decimal(18,4)");
        });
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        base.OnConfiguring(optionsBuilder);
    }
}
