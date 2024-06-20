using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using HE.Investments.Api.Auth;
using HE.Investments.Api.Config;
using HE.Investments.Api.Exceptions;
using HE.Investments.Api.Serialization;
using HE.Investments.Common.Contract.Exceptions;
using Polly;
using Polly.Retry;

namespace HE.Investments.Api;

public abstract class ApiHttpClientBase
{
    private readonly HttpClient _httpClient;

    private readonly IApiTokenProvider _apiTokenProvider;

    private readonly AsyncRetryPolicy<HttpResponseMessage> _httpPolicy;

    private readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        Converters = { new BoolConverter() },
    };

    protected ApiHttpClientBase(HttpClient httpClient, IApiTokenProvider apiTokenProvider, IApiConfig config)
    {
        _httpClient = httpClient;
        _apiTokenProvider = apiTokenProvider;
        _httpPolicy = Policy.HandleResult<HttpResponseMessage>(x => x.StatusCode is HttpStatusCode.RequestTimeout or >= HttpStatusCode.InternalServerError)
            .WaitAndRetryAsync(config.RetryCount, _ => TimeSpan.FromMilliseconds(config.RetryDelayInMilliseconds));
    }

    protected async Task<TResponse> SendAsync<TResponse>(
        string relativeUrl,
        HttpMethod method,
        CancellationToken cancellationToken)
    {
        return await SendAsync<TResponse>(CreateHttpRequestFactory(null, relativeUrl, method), cancellationToken);
    }

    protected async Task<TResponse> SendAsync<TRequest, TResponse>(
        TRequest request,
        string relativeUrl,
        HttpMethod method,
        CancellationToken cancellationToken)
    {
        var requestBody = JsonSerializer.Serialize(request, _jsonSerializerOptions);

        return await SendAsync<TResponse>(CreateHttpRequestFactory(requestBody, relativeUrl, method), cancellationToken);
    }

    private Func<Task<HttpRequestMessage>> CreateHttpRequestFactory(string? requestBody, string relativeUrl, HttpMethod method)
    {
        return async () =>
        {
            var request = new HttpRequestMessage(method, relativeUrl)
            {
                Headers = { Authorization = new AuthenticationHeaderValue("Bearer", await _apiTokenProvider.GetToken()) },
            };

            if (!string.IsNullOrEmpty(requestBody))
            {
                request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
            }

            return request;
        };
    }

    private async Task<TResponse> SendAsync<TResponse>(Func<Task<HttpRequestMessage>> httpRequestFactory, CancellationToken cancellationToken)
    {
        using var httpResponse = await SendHttpRequestAsync(httpRequestFactory, cancellationToken);
        if (!httpResponse.IsSuccessStatusCode)
        {
            var errorContent = await httpResponse.Content.ReadAsStringAsync(cancellationToken);
            throw new ApiCommunicationException(httpResponse.StatusCode, errorContent: errorContent);
        }

        var responseContent = await httpResponse.Content.ReadAsStringAsync(cancellationToken);
        if (string.IsNullOrEmpty(responseContent))
        {
            throw new NotFoundException($"Cannot find resource for {typeof(TResponse).Name}.");
        }

        try
        {
            return JsonSerializer.Deserialize<TResponse>(responseContent, _jsonSerializerOptions) ??
                   throw new ApiSerializationException(responseContent: responseContent);
        }
        catch (Exception ex) when (ex is JsonException or NotSupportedException or ArgumentNullException)
        {
            throw new ApiSerializationException(ex, responseContent);
        }
    }

    private async Task<HttpResponseMessage> SendHttpRequestAsync(Func<Task<HttpRequestMessage>> httpRequestFactory, CancellationToken cancellationToken)
    {
        return await _httpPolicy.ExecuteAsync(async () =>
        {
            using var httpRequest = await httpRequestFactory();
            return await _httpClient.SendAsync(httpRequest, cancellationToken);
        });
    }
}
