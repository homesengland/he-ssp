namespace HE.InvestmentLoans.Common.Models.App;

public class CacheConfig : ICacheConfig
{
    public string RedisConnectionString { get; set; }

    public int SessionExpireMinutes { get; set; }

    public int ExpireMinutes { get; set; }
}
