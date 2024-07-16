using System.Reflection;
using DfT.DTRO.DAL;
using DfT.DTRO.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DfT.DTRO.Extensions.DependencyInjection;

/// <summary>
/// Provides extension methods for injecting <see cref="IDtroService"/> implementations.
/// </summary>
public static class StorageServiceDIExtensions
{
    ///// <summary>
    ///// Adds PostgresDtroContext.
    ///// </summary>
    ///// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
    ///// <param name="connectionString">The connection string to the database.</param>
    ///// <returns>A reference to this instance after the operation has completed.</returns>
    //public static IServiceCollection AddPostgresStorage(this IServiceCollection services, string connectionString)
    //{
    //    services.AddPostgresDtroContext(connectionString);
    //    return services;
    //}

    /// <summary>
    /// Adds Storage.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
    /// <param name="configuration">The configuration that the storage service is inferred from.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    public static void AddStorage(this IServiceCollection services, IConfiguration configuration)
    {
        string assemblyName = Assembly.GetExecutingAssembly().GetName().Name;
        string connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<DtroContext>(options =>
            options.UseNpgsql(connectionString, builder =>
                builder.MigrationsAssembly(assemblyName)));
    }

    /// <summary>
    /// Create SWA entity.
    /// </summary>
    /// <param name="services">services parameter passed</param>
    public static ISwaSeeder RegisterSwaEntity(this IServiceCollection services)
    {
        ServiceProvider provider = services.BuildServiceProvider();
        return provider.GetRequiredService<ISwaSeeder>();
    }

    /// <summary>
    /// Seed SWA Codes into the newly created entity.
    /// </summary>
    /// <param name="seeder">seeder service passed</param>

    public static void AddSwaCodes(this IApplicationBuilder app, ISwaSeeder seeder) => seeder.Seed();

}