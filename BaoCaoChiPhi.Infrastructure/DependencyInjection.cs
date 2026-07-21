using BaoCaoChiPhi.Application.Interfaces;
using BaoCaoChiPhi.Domain.Interfaces;
using BaoCaoChiPhi.Infrastructure.Persistence;
using BaoCaoChiPhi.Infrastructure.Repositories;
using BaoCaoChiPhi.Infrastructure.Services;
using BaoCaoChiPhi.Infrastructure.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BaoCaoChiPhi.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddDbContext<ProductDataDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DbConnection")));

        services.AddDbContext<ProductFormDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DbConnection_Form")));

        services.Configure<DefaultUserSettings>(opts => configuration.GetSection("DefaultUser").Bind(opts));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IBaoCaoChiPhiRepository, BaoCaoChiPhiRepository>();
        services.AddScoped<IBienBanGiaoNhanRepository, BienBanGiaoNhanRepository>();
        services.AddScoped<IProductionDataRepository, ProductionDataRepository>();
        services.AddScoped<IAuthService, AuthService>();

        return services;
    }
}
