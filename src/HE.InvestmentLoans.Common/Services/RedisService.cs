using System.Net.Security;
using System.Runtime.InteropServices;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using HE.InvestmentLoans.Common.Models.App;
using HE.InvestmentLoans.Common.Services.Interfaces;
using StackExchange.Redis;

namespace HE.InvestmentLoans.Common.Services;

public class RedisService : ICacheService
{
    private readonly IAppConfig _appConfig;

    private readonly ConnectionMultiplexer _connection;

    public RedisService(IAppConfig appConfig, IDataverseConfig dataverseConfig)
    {
        _appConfig = appConfig;

        var configurationOptions = ConfigurationOptions.Parse(appConfig.Cache.RedisConnectionString);

        if (!string.IsNullOrEmpty(appConfig.Cache.RedisCertificatePath) &&
            !string.IsNullOrEmpty(appConfig.Cache.RedisCertificateKeyPath))
        {
#pragma warning disable CA5359
            configurationOptions.CertificateValidation += CertificateValidationCallBack!;
#pragma warning restore CA5359
            configurationOptions.CertificateSelection += OptionsOnCertificateSelection;
            configurationOptions.Ssl = true;
            configurationOptions.SslProtocols = SslProtocols.Tls12;
            configurationOptions.AbortOnConnectFail = false;
        }

        _connection = ConnectionMultiplexer.Connect(configurationOptions);
    }

    private IDatabase Cache => _connection.GetDatabase();

    public T? GetValue<T>(string key)
    {
        if (Cache.KeyExists(GetKey(key)))
        {
            string? resp = Cache.StringGet(GetKey(key));
            return resp != null ? JsonSerializer.Deserialize<T>(resp) : default;
        }

        return default;
    }

    public async Task<T?> GetValueAsync<T>(string key, Func<Task<T>> loadValue)
    {
        if (Cache.KeyExists(GetKey(key)))
        {
            return GetValue<T>(key);
        }

        var value = await loadValue();

        if (value != null)
        {
            SetValue(key, value);
        }

        return value;
    }

    public void SetValue(string key, object value) => SetValue(key, value, _appConfig.Cache.ExpireMinutes);

    public void SetValue(string key, object value, int expireMinutes)
    {
        Cache.StringSet(GetKey(key), JsonSerializer.Serialize(value), TimeSpan.FromMinutes(expireMinutes));
    }

    public void SetValue<T>(string key, T value)
    {
        SetValue(key, (object)value!);
    }

    private string GetKey(string key) => $"{_appConfig.AppName}_{key}";

    private X509Certificate OptionsOnCertificateSelection(object sender, string targethost, X509CertificateCollection localcertificates, X509Certificate? remotecertificate, string[] acceptableissuers)
    {
        return CreateCertFromPemFile(_appConfig.Cache.RedisCertificatePath, _appConfig.Cache.RedisCertificateKeyPath);
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
