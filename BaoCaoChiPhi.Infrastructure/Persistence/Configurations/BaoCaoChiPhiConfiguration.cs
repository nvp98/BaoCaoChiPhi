using BaoCaoChiPhi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BaoCaoChiPhi.Infrastructure.Persistence.Configurations;

public class BaoCaoChiPhiConfiguration : IEntityTypeConfiguration<BaoCaoChiPhiEntity>
{
    public void Configure(EntityTypeBuilder<BaoCaoChiPhiEntity> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.TieuDe)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.MoTa)
            .HasMaxLength(2000);

        builder.Property(x => x.TongTien)
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.NguoiLap)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasMany(x => x.ChiTiets)
            .WithOne()
            .HasForeignKey(x => x.BaoCaoId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
