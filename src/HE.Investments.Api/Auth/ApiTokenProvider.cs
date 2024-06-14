using HE.Investments.Api.Config;
using Microsoft.Identity.Client;

namespace HE.Investments.Api.Auth;

internal sealed class ApiTokenProvider
{
    private readonly IApiAuthConfig _config;

    public ApiTokenProvider(IApiAuthConfig config)
    {
        _config = config;
    }

    public async Task<ApiAccessToken> GetToken()
    {
        var app = ConfidentialClientApplicationBuilder
            .Create(_config.ClientId)
            .WithTenantId(_config.TenantId)
            .WithClientSecret(_config.ClientSecret)
            .Build();

        var result = await app.AcquireTokenForClient([_config.Scope]).ExecuteAsync();

        return new ApiAccessToken(result.AccessToken, result.ExpiresOn);
    }

    public sealed record ApiAccessToken(string AccessToken, DateTimeOffset ExpiresOn);
}
