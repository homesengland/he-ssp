using HE.Investments.Common.Models.App;
using Microsoft.Identity.Client;

namespace HE.Investments.Common.CRM.Services;

internal sealed class CrmApiTokenProvider
{
    private readonly ICrmApiAuthConfig _config;

    public CrmApiTokenProvider(ICrmApiAuthConfig config)
    {
        _config = config;
    }

    public async Task<CrmApiAccessToken> GetToken()
    {
        var app = ConfidentialClientApplicationBuilder
            .Create(_config.ClientId)
            .WithTenantId(_config.TenantId)
            .WithClientSecret(_config.ClientSecret)
            .Build();

        var result = await app.AcquireTokenForClient([_config.Scope]).ExecuteAsync();

        return new CrmApiAccessToken(result.AccessToken, result.ExpiresOn);
    }

    public record CrmApiAccessToken(string AccessToken, DateTimeOffset ExpiresOn);
}
