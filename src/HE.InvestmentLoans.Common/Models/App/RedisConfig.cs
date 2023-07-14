namespace HE.InvestmentLoans.Common.Models.App;

public class RedisConfig : IRedisConfig
{
    public string? ConnectionString { get; set; }

    public int ExpireMinutes { get; set; }
}
