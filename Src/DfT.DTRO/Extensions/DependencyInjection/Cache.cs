using StackExchange.Redis;

namespace DfT.DTRO.Extensions.DependencyInjection;

public static class Cache
{
    public static IServiceCollection AddCache(this IServiceCollection services, IConfiguration configuration)
    {
        return configuration.GetValue("EnableRedisCache", false)
            ? services.AddRedisCache(configuration)
            : services.AddSingleton<IRedisCache, NoopCache>();
    }

    private static IServiceCollection AddRedisCache(this IServiceCollection services, IConfiguration configuration)
    {
        var redisConfig = configuration.GetSection("Redis");

        var host = redisConfig.GetValue("Host", "localhost");
        var port = redisConfig.GetValue("Port", 6379);
        var password = redisConfig.GetValue<string>("Auth", null);
        var useSsl = redisConfig.GetValue("UseSsl", false);
        var certificateAuthorityCertPath = redisConfig.GetValue<string>("CaCertPath", null);

        return services.AddRedisCache(host, password, port, useSsl, certificateAuthorityCertPath);
    }

    private static IServiceCollection AddRedisCache(
        this IServiceCollection services,
        string host,
        string password,
        int port = 6379,
        bool useSsl = false,
        string certificateAuthorityCertPath = null)
    {
        return services
            .AddStackExchangeRedisCache(opt =>
            {
                opt.ConfigurationOptions = new ConfigurationOptions
                {
                    EndPoints = { $"{host ?? throw new ArgumentNullException(nameof(host))}:{port}" },
                    Ssl = useSsl
                };

                if (password is not null)
                {
                    opt.ConfigurationOptions.Password = password;
                }

                if (useSsl)
                {
                    opt.ConfigurationOptions.CertificateValidation += (_, certificate, _, _) =>
                        ValidateCertificate(certificateAuthorityCertPath, certificate);
                }
            })
            .AddScoped<IRedisCache, RedisCache>();
    }

    private static bool ValidateCertificate(string certificateAuthorityCertPath, X509Certificate certificate)
    {
        if (certificate == null)
        {
            return false;
        }

        X509Certificate2 ca = new(certificateAuthorityCertPath ??
                                   throw new ArgumentNullException(nameof(certificateAuthorityCertPath)));
        X509Certificate2 certificateToValidate = new(certificate);

        X509Chain x509Chain = new();
        x509Chain.ChainPolicy.RevocationMode = X509RevocationMode.NoCheck;
        x509Chain.ChainPolicy.RevocationFlag = X509RevocationFlag.ExcludeRoot;
        x509Chain.ChainPolicy.VerificationFlags =
            X509VerificationFlags.AllowUnknownCertificateAuthority;
        x509Chain.ChainPolicy.VerificationTime = DateTime.Now;
        x509Chain.ChainPolicy.UrlRetrievalTimeout = new TimeSpan(0, 0, 0);

        x509Chain.ChainPolicy.ExtraStore.Add(ca);

        bool isChainValid = x509Chain.Build(certificateToValidate);

        bool isSignedByCa = x509Chain.ChainElements
            .Any(element => element.Certificate.Thumbprint == ca.Thumbprint);

        return isChainValid && isSignedByCa;
    }
}
