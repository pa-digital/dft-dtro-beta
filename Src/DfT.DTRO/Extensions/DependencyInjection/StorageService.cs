using System.Reflection;
using DfT.DTRO.DAL;
using DfT.DTRO.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DfT.DTRO.Extensions.DependencyInjection;

public static class StorageServiceDIExtensions
{
    public static void AddStorage(this IServiceCollection services, IConfiguration configuration)
    {
        string assemblyName = Assembly.GetExecutingAssembly().GetName().Name;
        string connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<DtroContext>(options =>
            options.UseNpgsql(connectionString, builder =>
                builder.MigrationsAssembly(assemblyName)));
    }

    public static ISwaSeeder RegisterSwaEntity(this IServiceCollection services)
    {
        ServiceProvider provider = services.BuildServiceProvider();
        return provider.GetRequiredService<ISwaSeeder>();
    }

    public static void AddSwaCodes(this IApplicationBuilder app, ISwaSeeder seeder) => seeder.Seed();

}