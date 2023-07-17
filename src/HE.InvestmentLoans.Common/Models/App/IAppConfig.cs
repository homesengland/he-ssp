namespace HE.InvestmentLoans.Common.Models.App;

public interface IAppConfig
{
    public string? SupportEmail { get; set; }

    public RedisConfig? Redis { get; set; }

    public DataverseConfig? Dataverse { get; set; }

    public UrlConfig? Url { get; set; }

    public Auth0Config? Auth0 { get; set; }

    public CookieAuthenticationConfig? CookieAuthentication { get; set; }
}
