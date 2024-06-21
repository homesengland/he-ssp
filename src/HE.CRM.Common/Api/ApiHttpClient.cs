using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using HE.CRM.Common.Api.Exceptions;

namespace HE.CRM.Common.Api
{
    internal sealed class ApiHttpClient : IDisposable
    {
        private readonly HttpClient _httpClient;

        private readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        };

        public ApiHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<TResponse> SendAsync<TResponse>(string relativeUrl, HttpMethod method, CancellationToken cancellationToken)
            where TResponse : class
        {
            using (var httpRequest = CreateHttpRequest(null, relativeUrl, method))
            {
                return await SendAsync<TResponse>(httpRequest, cancellationToken);
            }
        }

        public async Task<TResponse> SendAsync<TRequest, TResponse>(
            TRequest request,
            string relativeUrl,
            HttpMethod method,
            CancellationToken cancellationToken)
            where TResponse : class
        {
            var requestBody = JsonSerializer.Serialize(request, _jsonSerializerOptions);
            using (var httpRequest = CreateHttpRequest(requestBody, relativeUrl, method))
            {
                return await SendAsync<TResponse>(httpRequest, cancellationToken);
            }
        }

        private static HttpRequestMessage CreateHttpRequest(string requestBody, string relativeUrl, HttpMethod method)
        {
            var request = new HttpRequestMessage(method, relativeUrl);
            if (!string.IsNullOrEmpty(requestBody))
            {
                request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
            }

            return request;
        }

        private async Task<TResponse> SendAsync<TResponse>(HttpRequestMessage httpRequest, CancellationToken cancellationToken)
            where TResponse : class
        {
            using (var httpResponse = await _httpClient.SendAsync(httpRequest, cancellationToken))
            {
                if (!httpResponse.IsSuccessStatusCode)
                {
                    var errorContent = await httpResponse.Content.ReadAsStringAsync();
                    throw new ApiCommunicationException(httpResponse.StatusCode, errorContent: errorContent);
                }

                var responseContent = await httpResponse.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(responseContent))
                {
                    throw new ApiSerializationException(responseContent: responseContent);
                }

                try
                {
                    return JsonSerializer.Deserialize<TResponse>(responseContent, _jsonSerializerOptions)
                           ?? throw new ApiSerializationException(responseContent: responseContent);
                }
                catch (JsonException ex)
                {
                    throw new ApiSerializationException(ex, responseContent);
                }
                catch (NotSupportedException ex)
                {
                    throw new ApiSerializationException(ex, responseContent);
                }
                catch (ArgumentNullException ex)
                {
                    throw new ApiSerializationException(ex, responseContent);
                }
            }
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}
