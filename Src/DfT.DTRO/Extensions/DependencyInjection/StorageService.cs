using System;
using DfT.DTRO.Services;
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
        var postgresConfig = configuration.GetRequiredSection("Postgres");

        var host = Environment.GetEnvironmentVariable("POSTGRES_HOST")
        ?? postgresConfig.GetValue<string>("Host");

        var port = int.TryParse(Environment.GetEnvironmentVariable("POSTGRES_PORT"), out int parsedPort)
        ? parsedPort : postgresConfig.GetValue<int?>("Port") ?? 5432;

        var user = Environment.GetEnvironmentVariable("POSTGRES_USER")
        ?? postgresConfig.GetValue<string>("User");

        var password = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD")
        ?? postgresConfig.GetValue<string>("Password");

        var database = Environment.GetEnvironmentVariable("POSTGRES_DATABASE")
        ?? postgresConfig.GetValue<string>("DbName");

        bool useSsl = bool.TryParse(Environment.GetEnvironmentVariable("POSTGRES_USE_SSL"), out bool parsedUseSsl)
        ? parsedUseSsl : postgresConfig.GetValue<bool>("UseSsl", false);

        int? maxPoolSize = int.TryParse(Environment.GetEnvironmentVariable("POSTGRES_MAX_POOL_SIZE"), out int parsedMaxPoolSize)
        ? parsedMaxPoolSize : postgresConfig.GetValue<int?>("MaxPoolSize", null);

        if (configuration.GetValue("WriteToBucket", false))
        {
            return
                services
                    .AddPostgresDtroContext(host, user, password, useSsl, database, port, maxPoolSize);
        }

        // if (configuration.GetValue("WriteToBucket", false))
        // {
        //    return
        //        services
        //            .AddPostgresDtroContext(host, user, password, useSsl, database, port)
        //            .AddMultiStorageService<SqlStorageService, FileStorageService>();
        // }
        return services.AddPostgresStorage(host, user, password, useSsl, database, port, maxPoolSize);
    }
}