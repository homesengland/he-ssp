using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using HE.Base.Services;
using HE.CRM.Common.Api.Auth.Contract;
using HE.CRM.Common.Repositories.Interfaces;

namespace HE.CRM.Common.Api.Auth
{
    public sealed class ApiTokenProvider : CrmService, IApiTokenProvider
    {
        private readonly IEnvironmentVariableRepository _variables;

        private string _accessToken;

        public ApiTokenProvider(CrmServiceArgs args)
            : base(args)
        {
            _variables = CrmRepositoriesFactory.GetSystem<IEnvironmentVariableRepository>();
        }

        public async Task<string> GetToken()
        {
            if (!string.IsNullOrEmpty(_accessToken))
            {
                return _accessToken;
            }

            Logger.Trace($"AzureAdHttpClientService.CheckAccessToken: executing");
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://login.microsoftonline.com");
                var request = new HttpRequestMessage(HttpMethod.Post, $"/{EnvironmentVariables.AzureAd.TenantId}/oauth2/token");

                var keyValues = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("grant_type", "client_credentials"),
                    new KeyValuePair<string, string>("client_id", _variables.GetEnvironmentVariableValue(EnvironmentVariables.AzureAd.ClientId)),
                    new KeyValuePair<string, string>("client_secret", _variables.GetEnvironmentVariableValue(EnvironmentVariables.AzureAd.ClientSecret)),
                    new KeyValuePair<string, string>("scope", $"api://{_variables.GetEnvironmentVariableValue(EnvironmentVariables.AzureAd.Scope)}/.default")
                };

                request.Content = new FormUrlEncodedContent(keyValues);
                var response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var adToken = Deserialize<AzureAdToken>(result);
                    _accessToken = adToken?.AccessToken;

                    Logger.Trace("ApiTokenProvider.GetToken: token retrieved from Azure AD");
                }
                else
                {
                    Logger.Error($"ApiTokenProvider.GetToken: Azure AD execution failed, response code: {response.StatusCode}");
                }
            }

            return _accessToken;
        }

        private static TEntity Deserialize<TEntity>(string json)
        {
            var serializer = new DataContractJsonSerializer(typeof(TEntity));
            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                return (TEntity)serializer.ReadObject(ms);
            }
        }
    }
}
