using DfT.DTRO.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

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
        var postgresConfig = configuration.GetRequiredSection("Postgres");

        var host = postgresConfig.GetValue<string>("Host");
        var port = postgresConfig.GetValue<int?>("Port") ?? 5432;
        var user = postgresConfig.GetValue<string>("User");
        var password = postgresConfig.GetValue<string>("Password");
        var database = postgresConfig.GetValue<string>("DbName");
        var useSsl = postgresConfig.GetValue("UseSsl", false);
        int? maxPoolSize = postgresConfig.GetValue<int?>("MaxPoolSize", null);

        return services.AddPostgresDtroContext(host, user, password, useSsl, database, port, maxPoolSize);
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
