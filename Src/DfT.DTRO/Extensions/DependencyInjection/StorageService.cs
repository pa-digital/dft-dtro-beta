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
        var postgresConfig = configuration.GetSection("Postgres");

        var host = postgresConfig.GetValue("Host", "localhost");
        var port = postgresConfig.GetValue("Port", 5432);
        var user = postgresConfig.GetValue("User", "root");
        var password = postgresConfig.GetValue("Password", "root");
        var database = postgresConfig.GetValue("DbName", "data");
        var useSsl = postgresConfig.GetValue("UseSsl", false);
        int? maxPoolSize = postgresConfig.GetValue<int?>("MaxPoolSize", null);

        if (configuration.GetValue("WriteToBucket", false))
        {
            return
                services
                    .AddPostgresDtroContext(host, user, password, useSsl, database, port);
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