using DfT.DTRO.Caching;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace DfT.DTRO.Extensions.DependencyInjection;

/// <summary>
/// Provides extension methods for registering DTRO cache.
/// </summary>
public static class Cache
{
    /// <summary>
    /// Adds an <see cref="IRedisCache"/> based on the provided configuration.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
    /// <param name="configuration">The cache configuration.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    public static IServiceCollection AddCache(this IServiceCollection services, IConfiguration configuration)
    {
        return configuration.GetValue("EnableRedisCache", false)
            ? services.AddRedisCache(configuration)
            : services.AddSingleton<IRedisCache, NoopCache>();
    }

    /// <summary>
    /// Adds an <see cref="IRedisCache"/> with a Redis backend.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
    /// <param name="configuration">The configuration to get connection parameters from.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
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

    /// <summary>
    /// Adds an <see cref="IRedisCache"/> with a Redis backend.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
    /// <param name="host">The Redis host address.</param>
    /// <param name="password">The Redis password.</param>
    /// <param name="port">The Redis host port.</param>
    /// <param name="useSsl">Enables encryption of connection with SSL/TLS.</param>
    /// <param name="certificateAuthorityCertPath">Path to CA that signed Redis server certificate.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
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

        X509Certificate2 ca = new (certificateAuthorityCertPath ??
                                   throw new ArgumentNullException(nameof(certificateAuthorityCertPath)));
        X509Certificate2 certificateToValidate = new (certificate);

        X509Chain x509Chain = new ();
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
