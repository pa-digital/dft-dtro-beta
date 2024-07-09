using System;
using DfT.DTRO.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DfT.DTRO.Extensions.DependencyInjection;

/// <summary>
/// Provides extension methods for injecting <see cref="IDtroService"/> implementations.
/// </summary>
public static class StorageServiceDIExtensions
{
    /// <summary>
    /// Adds PostgresDtroContext.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
    /// <param name="connectionString">The connection string to the database.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    public static IServiceCollection AddPostgresStorage(this IServiceCollection services, string connectionString)
    {
        services.AddPostgresDtroContext(connectionString);
        return services;
    }

    /// <summary>
    /// Adds Storage.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
    /// <param name="configuration">The configuration that the storage service is infered from.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    public static IServiceCollection AddStorage(this IServiceCollection services, IConfiguration configuration)
    {
        var postgresConfig = configuration.GetSection("Postgres");

        var host = Environment.GetEnvironmentVariable("POSTGRES_HOST")
            ?? postgresConfig.GetValue("Host", "localhost");

        var port = int.TryParse(Environment.GetEnvironmentVariable("POSTGRES_PORT"), out int envPort)
            ? envPort : postgresConfig.GetValue("Port", 5432);

        var user = Environment.GetEnvironmentVariable("POSTGRES_USER")
            ?? postgresConfig.GetValue("User", "postgres");

        var password = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD")
            ?? postgresConfig.GetValue("Password", "admin");

        var database = Environment.GetEnvironmentVariable("POSTGRES_DB")
            ?? postgresConfig.GetValue("DbName", "data");

        var useSsl = bool.TryParse(Environment.GetEnvironmentVariable("POSTGRES_USE_SSL"), out bool envUseSsl)
            ? envUseSsl : postgresConfig.GetValue("UseSsl", false);

        int? maxPoolSize = int.TryParse(Environment.GetEnvironmentVariable("POSTGRES_MAX_POOL_SIZE"), out int envMaxPoolSize)
            ? (int?)envMaxPoolSize : postgresConfig.GetValue<int?>("MaxPoolSize", null);

        var writeToBucket = bool.TryParse(Environment.GetEnvironmentVariable("WRITE_TO_BUCKET"), out bool envWriteToBucket)
            ? envWriteToBucket : configuration.GetValue("WriteToBucket", false);

        if (writeToBucket)
        {
            return services.AddPostgresDtroContext(host, user, password, useSsl, database, port);
        }

        return services.AddPostgresStorage(host, user, password, useSsl, database, port, maxPoolSize);
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