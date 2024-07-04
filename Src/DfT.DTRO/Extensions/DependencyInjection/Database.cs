using System;
using Npgsql;
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
        return services.AddDbContext<DtroContext>(opt => opt.UseNpgsql(connectionString));
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
        => services.AddPostgresDtroContext(BuildPostgresConnectionString(host, user, password, useSsl, database, port, maxPoolSize));

    /// <summary>
    /// Adds an <see cref="DtroContext"/> using the connection parameters contained in the <paramref name="configuration"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
    /// <param name="configuration">The configuration that the connection parameters are infered from.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    public static IServiceCollection AddPostgresDtroContext(this IServiceCollection services, IConfiguration configuration)
    {
        var host = Environment.GetEnvironmentVariable("POSTGRES_HOST") ??
            configuration.GetValue("Postgres:Host", "localhost");

        var port = int.TryParse(Environment.GetEnvironmentVariable("POSTGRES_PORT"), out int envPort)
            ? envPort : configuration.GetValue("Postgres:Port", 5432);

        var user = Environment.GetEnvironmentVariable("POSTGRES_USER")
            ?? configuration.GetValue("Postgres:User", "postgres");

        var password = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD")
            ?? configuration.GetValue("Postgres:Password", "admin");

        var database = Environment.GetEnvironmentVariable("POSTGRES_DB")
            ?? configuration.GetValue("Postgres:DbName", "data");

        var useSsl = bool.TryParse(Environment.GetEnvironmentVariable("POSTGRES_USE_SSL"), out bool envUseSsl)
            ? envUseSsl : configuration.GetValue("Postgres:UseSsl", false);

        int? maxPoolSize = int.TryParse(Environment.GetEnvironmentVariable("POSTGRES_MAX_POOL_SIZE"), out int envMaxPoolSize)
            ? envMaxPoolSize : configuration.GetValue<int?>("Postgres:MaxPoolSize", null);

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
        => services.AddPostgresStorage(BuildPostgresConnectionString(host, user, password, useSsl, database, port, maxPoolSize));


    /// <summary>
    /// Builds a PostgreSQL connection string using the provided parameters.
    /// </summary>
    /// <param name="host">The address of the database host.</param>
    /// <param name="user">The username used to log in.</param>
    /// <param name="password">The password used to log in.</param>
    /// <param name="useSsl">Enables encryption of connection with SSL/TLS.</param>
    /// <param name="database">The name of the database. By default the same as <paramref name="user"/>.</param>
    /// <param name="port">The port used to connect to the database. By default <c>5432</c>.</param>
    /// <param name="maxPoolSize">The maximum size of the connection pool.</param>
    /// <returns>The built PostgreSQL connection string.</returns>
    internal static string BuildPostgresConnectionString(
        string host,
        string user,
        string password,
        bool useSsl,
        string database = null,
        int port = 5432,
        int? maxPoolSize = null)
    {
        var connectionStringBuilder = new NpgsqlConnectionStringBuilder
        {
            Host = host,
            Port = port,
            Username = user,
            Password = password,
            Database = database ?? user,
            SslMode = SslMode.Disable,
        };

        if (maxPoolSize.HasValue)
        {
            connectionStringBuilder.Pooling = true;
            connectionStringBuilder.MaxPoolSize = maxPoolSize.Value;
        }

        return connectionStringBuilder.ToString();

    }

}
