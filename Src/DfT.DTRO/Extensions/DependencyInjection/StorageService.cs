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
    var host = Environment.GetEnvironmentVariable("POSTGRES_HOST");
    var port = Environment.GetEnvironmentVariable("POSTGRES_PORT");
    var user = Environment.GetEnvironmentVariable("POSTGRES_USER");
    var password = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD");
    var database = Environment.GetEnvironmentVariable("POSTGRES_DATABASE");
    bool useSsl = bool.TryParse(Environment.GetEnvironmentVariable("POSTGRES_USE_SSL"), out bool parsedUseSsl) ? parsedUseSsl : false;
    int? maxPoolSize = int.TryParse(Environment.GetEnvironmentVariable("POSTGRES_MAX_POOL_SIZE"), out int parsedMaxPoolSize) ? parsedMaxPoolSize : null;

    // If environment variables are not set, fallback to configuration values
    if (string.IsNullOrWhiteSpace(host) || string.IsNullOrWhiteSpace(port)
    || string.IsNullOrWhiteSpace(user) || string.IsNullOrWhiteSpace(password)
    || string.IsNullOrWhiteSpace(database))
    {
        var postgresConfig = configuration.GetRequiredSection("Postgres");

        host = postgresConfig.GetValue<string>("Host");
        port = configuration.GetValue<int?>("Port")?.ToString() ?? "5432";
        user = postgresConfig.GetValue<string>("User");
        password = postgresConfig.GetValue<string>("Password");
        database = postgresConfig.GetValue<string>("DbName");
        useSsl = postgresConfig.GetValue<bool>("UseSsl", false);
        maxPoolSize = postgresConfig.GetValue<int?>("MaxPoolSize");
    }

    if (configuration.GetValue("WriteToBucket", false))
    {
        return services.AddPostgresDtroContext(host, user, password, useSsl, database, int.Parse(port), maxPoolSize);
    }

    return services.AddPostgresStorage(host, user, password, useSsl, database, int.Parse(port), maxPoolSize);
}
}