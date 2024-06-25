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
        var host = Environment.GetEnvironmentVariable("POSTGRES_HOST") ?? configuration.GetValue<string>("Postgres:Host");
        var port = int.TryParse(Environment.GetEnvironmentVariable("POSTGRES_PORT"), out int parsedPort) ? parsedPort : configuration.GetValue<int>("Postgres:Port");
        var user = Environment.GetEnvironmentVariable("POSTGRES_USER") ?? configuration.GetValue<string>("Postgres:User");
        var password = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD") ?? configuration.GetValue<string>("Postgres:Password");
        var database = Environment.GetEnvironmentVariable("POSTGRES_DATABASE") ?? configuration.GetValue<string>("Postgres:Database");
        bool useSsl = bool.TryParse(Environment.GetEnvironmentVariable("POSTGRES_USE_SSL"), out bool parsedUseSsl) ? parsedUseSsl : configuration.GetValue<bool>("Postgres:UseSsl");
        int? maxPoolSize = int.TryParse(Environment.GetEnvironmentVariable("POSTGRES_MAX_POOL_SIZE"), out int parsedMaxPoolSize) ? parsedMaxPoolSize : configuration.GetValue<int?>("Postgres:MaxPoolSize");

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