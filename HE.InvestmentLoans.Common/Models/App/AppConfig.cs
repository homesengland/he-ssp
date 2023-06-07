namespace HE.InvestmentLoans.Common.Models.App;

public interface IAppConfig
{
    public string? SupportEmail { get; set; }
    public string? RedisConnectionString { get; set; }
    public DataverseConfig? Dataverse { get; set; }
    public UrlConfig? Url { get; set; }
    public Auth0Config? Auth0 { get; set; }
    public CookieAuthenticationConfig? CookieAuthentication { get; set; }
}

public class AppConfig : IAppConfig
{
    public string? SupportEmail { get; set; }
    public string? RedisConnectionString { get; set; }
    public DataverseConfig? Dataverse { get; set; }
    public UrlConfig? Url { get; set; }
    public Auth0Config? Auth0 { get; set; }
    public CookieAuthenticationConfig? CookieAuthentication { get; set; }
}
