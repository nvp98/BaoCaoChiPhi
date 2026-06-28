using BaoCaoChiPhi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BaoCaoChiPhi.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<BaoCaoChiPhiEntity> BaoCaoChiPhis => Set<BaoCaoChiPhiEntity>();
    public DbSet<ChiTietChiPhi> ChiTietChiPhis => Set<ChiTietChiPhi>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
