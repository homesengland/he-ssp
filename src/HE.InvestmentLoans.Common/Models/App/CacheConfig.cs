namespace HE.InvestmentLoans.Common.Models.App;

public class CacheConfig : ICacheConfig
{
    public bool? RedisCertificateEnabled { get; set; }

    public string RedisCertificatePath { get; set; }

    public string RedisCertificateKeyPath { get; set; }

    public string RedisConnectionString { get; set; }

    public int SessionExpireMinutes { get; set; }

    public int ExpireMinutes { get; set; }
}
