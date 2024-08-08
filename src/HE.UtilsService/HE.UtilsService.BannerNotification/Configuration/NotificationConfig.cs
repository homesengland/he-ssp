namespace HE.UtilsService.BannerNotification.Configuration;

public class NotificationConfig : INotificationConfig
{
    public string ChannelPrefix { get; set; }

    public bool RedisCertificateEnabled { get; set; }

    public string RedisCertificatePath { get; set; }

    public string RedisCertificateKeyPath { get; set; }

    public string RedisConnectionString { get; set; }

    public int? NotificationExpirationTimeInHours { get; set; }
}
