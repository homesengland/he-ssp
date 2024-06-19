using System.Threading.Tasks;
using HE.CRM.Common.Api.Config;
using Microsoft.Identity.Client;

namespace HE.CRM.Common.Api.Auth
{
    public class ApiTokenProvider : IApiTokenProvider
    {
        private readonly IApiAuthConfig _config;

        public ApiTokenProvider(IApiAuthConfig config)
        {
            _config = config;
        }

        public async Task<string> GetToken()
        {
            var app = ConfidentialClientApplicationBuilder
                .Create(_config.ClientId)
                .WithTenantId(_config.TenantId)
                .WithClientSecret(_config.ClientSecret)
                .Build();

            var result = await app.AcquireTokenForClient(new[] { _config.Scope }).ExecuteAsync();

            return result.AccessToken;
        }
    }
}
