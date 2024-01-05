using HE.Investments.Common.Config;
using HE.Investments.Common.CRM.Config;
using HE.Investments.Common.Infrastructure.Cache.Config;

namespace HE.Investments.Loans.Common.Models.App;

public interface IAppConfig
{
    public string? SupportEmail { get; set; }

    public string? FundingSupportEmail { get; set; }

    public string? AppName { get; set; }

    public CacheConfig Cache { get; set; }

    public DataverseConfig? Dataverse { get; set; }

    public UrlConfig? Url { get; set; }

    public Auth0Config? Auth0 { get; set; }

    public CookieAuthenticationConfig? CookieAuthentication { get; set; }
}
