namespace HE.UtilsService.BannerNotification.Storage.Redis;

public interface IRedisConfig
{
    public string ChannelPrefix { get; }

    public bool RedisCertificateEnabled { get; }

    public string RedisCertificatePath { get; }

    public string RedisCertificateKeyPath { get; }

    public string RedisConnectionString { get; }
}
