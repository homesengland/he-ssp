using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using HE.Base.Log;

namespace HE.CRM.Common.Api.Auth.AzureAd
{
    internal sealed class AzureAdTokenProvider : ITokenProvider
    {
        private readonly IBaseLogger _logger;

        private readonly AzureAdAuthConfig _config;

        private string _accessToken;

        public AzureAdTokenProvider(IBaseLogger logger, AzureAdAuthConfig config)
        {
            _logger = logger;
            _config = config;
        }

        public string GetToken()
        {
            if (!string.IsNullOrEmpty(_accessToken))
            {
                return _accessToken;
            }

            _logger.Trace($"AzureAdHttpClientService.CheckAccessToken: executing");
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://login.microsoftonline.com");
                client.Timeout = TimeSpan.FromSeconds(10);
                client.DefaultRequestHeaders.ConnectionClose = true;

                var formValues = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("grant_type", "client_credentials"),
                    new KeyValuePair<string, string>("client_id", _config.ClientId),
                    new KeyValuePair<string, string>("client_secret", _config.ClientSecret),
                    new KeyValuePair<string, string>("scope", $"api://{_config.Scope}/.default")
                };
                var request = new HttpRequestMessage(HttpMethod.Post, $"/{_config.TenantId}/oauth2/v2.0/token")
                {
                    Content = new FormUrlEncodedContent(formValues)
                };

                var response = client.SendAsync(request).GetAwaiter().GetResult();
                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    var adToken = Deserialize<AzureAdToken>(result);
                    _accessToken = adToken?.AccessToken;

                    _logger.Trace("ApiTokenProvider.GetToken: token retrieved from Azure AD");
                }
                else
                {
                    var responceContent = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    _logger.Error($"ApiTokenProvider.GetToken: Azure AD execution failed, response code: {response.StatusCode}");
                    _logger.Error(responceContent);
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
