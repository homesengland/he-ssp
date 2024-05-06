using System.Diagnostics.CodeAnalysis;
using System.Net.Security;
using System.Runtime.InteropServices;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using HE.Investments.Common.Infrastructure.Cache.Config;
using StackExchange.Redis;

namespace HE.Investments.Common.WWW.Infrastructure.Cache;

[SuppressMessage("Security", "CA5359", Justification = "It's need to be fixed?")]
public class RedisConfigurationOptions
{
    private readonly ICacheConfig _cacheConfig;

    public RedisConfigurationOptions(ICacheConfig cacheConfig, string envPrefix)
    {
        _cacheConfig = cacheConfig;

        ConfigurationOptions = ConfigurationOptions.Parse(cacheConfig.RedisConnectionString);

        if (cacheConfig.RedisCertificateEnabled == true)
        {
            ConfigurationOptions.CertificateValidation += CertificateValidationCallBack!;
            ConfigurationOptions.CertificateSelection += OptionsOnCertificateSelection;
            ConfigurationOptions.Ssl = true;
            ConfigurationOptions.SslProtocols = SslProtocols.Tls12;
            ConfigurationOptions.AbortOnConnectFail = false;
            ConfigurationOptions.ChannelPrefix = new RedisChannel(envPrefix, RedisChannel.PatternMode.Auto);
        }
    }

    public ConfigurationOptions ConfigurationOptions { get; }

    private X509Certificate2 OptionsOnCertificateSelection(object sender, string targethost, X509CertificateCollection localcertificates, X509Certificate? remotecertificate, string[] acceptableissuers)
    {
        return CreateCertFromPemFile(_cacheConfig.RedisCertificatePath, _cacheConfig.RedisCertificateKeyPath);
    }

    private X509Certificate2 CreateCertFromPemFile(string certPath, string keyPath)
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return X509Certificate2.CreateFromPemFile(certPath, keyPath);
        }

        using var cert = X509Certificate2.CreateFromPemFile(certPath, keyPath);
        return new X509Certificate2(cert.Export(X509ContentType.Pkcs12));
    }

    private bool CertificateValidationCallBack(
        object sender,
        X509Certificate certificate,
        X509Chain? chain,
        SslPolicyErrors sslPolicyErrors)
    {
        return true;
    }
}
