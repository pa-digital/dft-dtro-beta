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
        // Attempt to retrieve the configuration values from environment variables first, then fall back to appsettings.json if not found
        var host = Environment.GetEnvironmentVariable("POSTGRES_HOST")
        ?? configuration.GetValue<string>("Postgres:Host", "localhost");

        int port = int.TryParse(Environment.GetEnvironmentVariable("POSTGRES_PORT"), out int portValue)
        ? portValue : configuration.GetValue<int>("Postgres:Port", 5432);

        var user = Environment.GetEnvironmentVariable("POSTGRES_USER")
        ?? configuration.GetValue<string>("Postgres:User" , "postgres");

        var password = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD")
        ?? configuration.GetValue<string>("Postgres:Password", "admin");

        var database = Environment.GetEnvironmentVariable("POSTGRES_DBNAME")
        ?? configuration.GetValue<string>("Postgres:DbName", "data");

        bool useSsl = bool.TryParse(Environment.GetEnvironmentVariable("POSTGRES_USE_SSL"), out bool useSslValue)
        ? useSslValue : configuration.GetValue("Postgres:UseSsl", false);

        int? maxPoolSize = int.TryParse(Environment.GetEnvironmentVariable("POSTGRES_MAX_POOL_SIZE"), out int maxPoolSizeValue)
        ? maxPoolSizeValue : configuration.GetValue<int?>("Postgres:MaxPoolSize", null);

        if (configuration.GetValue("WriteToBucket", false))
        {
            return services.AddPostgresDtroContext(host, user, password, useSslValue, database, portValue);
        }

        return services.AddPostgresStorage(host, user, password, useSslValue, database, portValue, maxPoolSizeValue);
    }


}