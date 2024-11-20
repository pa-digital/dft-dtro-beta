namespace DfT.DTRO.Extensions.DependencyInjection;

public static class StorageService
{
    public static void AddStorage(this IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = Build(services, configuration);
        services.AddDbContext<DtroContext>(options =>
            options.UseNpgsql(connectionString, builder =>
            {
                string assemblyName = Assembly.GetExecutingAssembly().GetName().Name;
                builder.MigrationsAssembly(assemblyName);
            }));
    }

    private static string Build(IServiceCollection services, IConfiguration configuration)
    {
        string host = Environment.GetEnvironmentVariable("POSTGRES_HOST") ??
                      configuration.GetValue("Postgres:Host", "localhost");

        int port = int.TryParse(Environment.GetEnvironmentVariable("POSTGRES_PORT"), out int envPort)
            ? envPort : configuration.GetValue("Postgres:Port", 5432);

        string user = Environment.GetEnvironmentVariable("POSTGRES_USER")
                      ?? configuration.GetValue("Postgres:User", "postgres");

        string password = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD")
                          ?? configuration.GetValue("Postgres:Password", "admin");

        string database = Environment.GetEnvironmentVariable("POSTGRES_DB")
                          ?? configuration.GetValue("Postgres:DbName", "data");

        bool useSsl = bool.TryParse(Environment.GetEnvironmentVariable("POSTGRES_USE_SSL"), out bool envUseSsl)
            ? envUseSsl : configuration.GetValue("Postgres:UseSsl", false);

        int? maxPoolSize = int.TryParse(Environment.GetEnvironmentVariable("POSTGRES_MAX_POOL_SIZE"), out int envMaxPoolSize)
            ? envMaxPoolSize : configuration.GetValue<int?>("Postgres:MaxPoolSize", null);

        return services.AddPostgresContext(host, port, user, password, database, useSsl, maxPoolSize);
    }

    private static string AddPostgresContext(this IServiceCollection services, string host, int port,
        string user,
        string password, string database, bool useSsl, int? maxPoolSize)
    {
        NpgsqlConnectionStringBuilder connectionStringBuilder = new()
        {
            Host = host,
            Port = port,
            Username = user,
            Password = password,
            Database = database ?? user,
            SslMode = SslMode.Disable,
        };

        if (!maxPoolSize.HasValue)
        {
            return connectionStringBuilder.ToString();
        }

        connectionStringBuilder.Pooling = true;
        connectionStringBuilder.MaxPoolSize = maxPoolSize.Value;

        return connectionStringBuilder.ToString();
    }
}