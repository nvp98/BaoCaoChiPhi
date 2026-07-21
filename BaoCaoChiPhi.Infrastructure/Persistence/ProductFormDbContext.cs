using BaoCaoChiPhi.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace BaoCaoChiPhi.Infrastructure.Persistence;

public class ProductFormDbContext(DbContextOptions<ProductFormDbContext> options) : DbContext(options)
{
    public DbSet<ChiPhiProductionData> ChiPhiProductionData => Set<ChiPhiProductionData>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ChiPhiProductionData>(e =>
        {
            e.ToTable("ChiPhi_ProductionData", "dbo");
            e.HasKey(x => x.Id);
            e.Property(x => x.KhoiLuong).HasColumnType("decimal(18,3)");
        });
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        base.OnConfiguring(optionsBuilder);
    }
}
