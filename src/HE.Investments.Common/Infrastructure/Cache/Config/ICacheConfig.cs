namespace HE.Investments.Common.Infrastructure.Cache.Config;

public interface ICacheConfig
{
    public bool? RedisCertificateEnabled { get; set; }

    public string RedisCertificatePath { get; set; }

    public string RedisCertificateKeyPath { get; set; }

    public string RedisConnectionString { get; set; }

    public int SessionExpireMinutes { get; set; }

    public int ExpireMinutes { get; set; }
}
