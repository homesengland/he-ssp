using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using HE.CRM.Common.Repositories;
using HE.CRM.Plugins.Models;
using HE.Base.Services;

namespace HE.CRM.Plugins.Services.HttpService
{
    public static class CacheKeys
    {
        public const string AzureIntegrationApiEndpoint = "_HE/AzureIntegrationApiEndpoint";
        public const string AzureCalculationApiEndpoint = "_HE/AzureCalculationApiEndpoint";
        public const string AzureAdTokenId = "_HE/AzureAdTokenId";
        public const string AzureAdClientId = "_HE/AzureAdClientId";
        public const string AzureAdClientSecret = "_HE/AzureAdClientSecret";
    }

    public abstract class AzureAdHttpClientService : CrmService
    {
        private readonly HttpClient azureAdHttpClient;
        //private readonly ISiteSettingsRepository siteSettingsRepository;

        protected Uri IntegrationApiUrl => new Uri(GetSiteSettingValue(CacheKeys.AzureIntegrationApiEndpoint));
        protected Uri CalculationApiUrl => new Uri(GetSiteSettingValue(CacheKeys.AzureCalculationApiEndpoint));
        protected string AzureAdTokenId => GetSiteSettingValue(CacheKeys.AzureAdTokenId);
        protected string AzureAdClientId => GetSiteSettingValue(CacheKeys.AzureAdClientId);
        protected string AzureAdClientSecret => GetSiteSettingValue(CacheKeys.AzureAdClientSecret);

        public AzureAdHttpClientService(CrmServiceArgs args) : base(args)
        {
            //siteSettingsRepository = CrmRepositoriesFactory.Get<ISiteSettingsRepository>();

            azureAdHttpClient = new HttpClient();
            azureAdHttpClient.DefaultRequestHeaders.Accept.Clear();
            azureAdHttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public Task<HttpResponseMessage> PostAsync(Uri apiUri, string requestUri, HttpContent content)
        {
            azureAdHttpClient.BaseAddress = apiUri;
            SetAuthorizationToken();
            return azureAdHttpClient.PostAsync(requestUri, content);
        }

        public string Serialize<TEntity>(TEntity entity)
        {
            using (var ms = new MemoryStream())
            {
                var serializer = new DataContractJsonSerializer(typeof(TEntity));
                serializer.WriteObject(ms, entity);
                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }

        public TEntity Deserialize<TEntity>(string json)
        {
            var serializer = new DataContractJsonSerializer(typeof(TEntity));
            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                return (TEntity)serializer.ReadObject(ms);
            }
        }

        private void SetAuthorizationToken()
        {
            var accessToken = string.Empty;

            base.Logger.Trace($"AzureAdHttpClientService.CheckAccessToken: executing");
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://login.microsoftonline.com");
                var request = new HttpRequestMessage(HttpMethod.Post, $"/{AzureAdTokenId}/oauth2/token");

                var keyValues = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("grant_type", "client_credentials"),
                    new KeyValuePair<string, string>("client_id", AzureAdClientId),
                    new KeyValuePair<string, string>("client_secret", AzureAdClientSecret),
                    new KeyValuePair<string, string>("resource", "https://graph.windows.net/")
                };

                request.Content = new FormUrlEncodedContent(keyValues);
                var response = client.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();

                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                    var adToken = Deserialize<AzureAdToken>(result);
                    accessToken = adToken?.access_token;

                    Logger.Trace($"AzureAdHttpClientService.CheckAccessToken: token retrieved from Azure AD: {accessToken != null}");
                }
                else
                {
                    Logger.Error($"AzureAdHttpClientService.CheckAccessToken: Azure AD execution failed, token is missing");
                }
            }

            azureAdHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }

        private string GetSiteSettingValue(string key)
        {
            //string cacheEntry = siteSettingsRepository.GetByName(key)?.Adx_Value;
            base.Logger.Trace($"AzureAdHttpClientService.GetSiteSettingValue retrieved key from crm: '{key}'");
            return "";//cacheEntry;
        }
    }
}
