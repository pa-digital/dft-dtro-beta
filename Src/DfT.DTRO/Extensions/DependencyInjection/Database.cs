using System;
using DfT.DTRO.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DfT.DTRO.Extensions.DependencyInjection;

/// <summary>
/// Provides extension methods for registering the database connection.
/// </summary>
public static class Database
{
    /// <summary>
    /// Adds <see cref="DtroContext"/> with PostgreSQL
    /// backend using the provided connection string.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
    /// <param name="connectionString">The connection string to the database.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    public static IServiceCollection AddPostgresDtroContext(this IServiceCollection services, string connectionString)
    {
        return services.AddDbContext<DtroContext>(
            opt => opt.UseNpgsql(connectionString));
    }

    /// <summary>
    /// Adds <see cref="DtroContext"/> with PostgreSQL
    /// backend using the provided connection string.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
    /// <param name="host">The address of the database host.</param>
    /// <param name="user">The username used to log in.</param>
    /// <param name="password">The password used to log in.</param>
    /// <param name="useSsl">Enables encryption of connection with SSL/TLS.</param>
    /// <param name="database">The name of the database. By default the same as <paramref name="user"/>.</param>
    /// <param name="port">The port used to connect to the database. By default <c>5432</c>.</param>
    /// <param name="maxPoolSize">The maximum size of the connection pool.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    public static IServiceCollection AddPostgresDtroContext(
        this IServiceCollection services,
        string host,
        string user,
        string password,
        bool useSsl,
        string database = null,
        int port = 5432,
        int? maxPoolSize = null)
        => services.AddPostgresDtroContext(
            BuildPostgresConnectionString(host, user, password, useSsl, database, port, maxPoolSize));

    /// <summary>
    /// Adds an <see cref="DtroContext"/> using the connection parameters contained in the <paramref name="configuration"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
    /// <param name="configuration">The configuration that the connection parameters are infered from.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
public static IServiceCollection AddPostgresDtroContext(this IServiceCollection services, IConfiguration configuration)
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

    return services.AddPostgresDtroContext(host, user, password, useSsl, database, int.Parse(port), maxPoolSize);
}

    /// <summary>
    /// with PostgreSQL backend using the provided connection parameters.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
    /// <param name="host">The address of the database host.</param>
    /// <param name="user">The username used to log in.</param>
    /// <param name="password">The password used to log in.</param>
    /// <param name="useSsl">Enables encryption of connection with SSL/TLS.</param>
    /// <param name="database">The name of the database. By default the same as <paramref name="user"/>.</param>
    /// <param name="port">The port used to connect to the database. By defauls <c>5432</c>.</param>
    /// <param name="maxPoolSize">The maximum size of the connection pool.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    public static IServiceCollection AddPostgresStorage(
        this IServiceCollection services,
        string host,
        string user,
        string password,
        bool useSsl,
        string database = null,
        int port = 5432,
        int? maxPoolSize = null)
        => services.AddPostgresStorage(
            BuildPostgresConnectionString(host, user, password, useSsl, database, port, maxPoolSize));

    internal static string BuildPostgresConnectionString(
        string host,
        string user,
        string password,
        bool useSsl,
        string database = null,
        int port = 5432,
        int? maxPoolSize = null)
        => $"Host={host ?? throw new ArgumentNullException(nameof(host))}:{port};" +
           $"Username={user ?? throw new ArgumentNullException(nameof(user))};" +
           $"Password={password ?? throw new ArgumentNullException(nameof(password))};" +
           $"Database={database ?? user};" +
           $"{(useSsl ? "sslmode=Require;Trust Server Certificate=true;" : string.Empty)};" +
           $"{(maxPoolSize is not null ? $"Maximum Pool Size={maxPoolSize};" : string.Empty)};";
}
