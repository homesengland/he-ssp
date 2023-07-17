namespace HE.InvestmentLoans.Common.Models.App;

public interface IRedisConfig
{
    public string ConnectionString { get; set; }

    public int ExpireMinutes { get; set; }
}
