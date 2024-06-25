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
    /// <returns>A reference to this instance after the operation has completed.</returns>
    public static IServiceCollection AddStorage(this IServiceCollection services)
    {

        var host = Environment.GetEnvironmentVariable("POSTGRES_HOST");
        var port = Environment.GetEnvironmentVariable("POSTGRES_PORT");
        var user = Environment.GetEnvironmentVariable("POSTGRES_USER");
        var password = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD");
        var database = Environment.GetEnvironmentVariable("POSTGRES_DATABASE");
        var useSsl = Environment.GetEnvironmentVariable("POSTGRES_USE_SSL");
        var maxPoolSize = Environment.GetEnvironmentVariable("POSTGRES_MAX_POOL_SIZE");

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