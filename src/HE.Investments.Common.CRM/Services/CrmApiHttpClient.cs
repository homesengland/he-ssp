using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using HE.Investments.Common.CRM.Exceptions;
using HE.Investments.Common.Models.App;
using Polly;
using Polly.Retry;

namespace HE.Investments.Common.CRM.Services;

public abstract class CrmApiHttpClient
{
    private readonly HttpClient _httpClient;

    private readonly ICrmApiTokenProvider _crmApiTokenProvider;

    private readonly AsyncRetryPolicy<HttpResponseMessage> _httpPolicy;

    private readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    };

    protected CrmApiHttpClient(HttpClient httpClient, ICrmApiTokenProvider crmApiTokenProvider, ICrmApiConfig config)
    {
        _httpClient = httpClient;
        _crmApiTokenProvider = crmApiTokenProvider;
        _httpPolicy = Policy.HandleResult<HttpResponseMessage>(x => x.StatusCode is HttpStatusCode.RequestTimeout or >= HttpStatusCode.InternalServerError)
            .WaitAndRetryAsync(config.RetryCount, _ => TimeSpan.FromMilliseconds(config.RetryDelayInMilliseconds));
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

    private Func<Task<HttpRequestMessage>> CreateHttpRequestFactory(string requestBody, string relativeUrl, HttpMethod method)
    {
        return async () => new(method, relativeUrl)
        {
            Headers = { Authorization = new AuthenticationHeaderValue("Bearer", await _crmApiTokenProvider.GetToken()) },
            Content = new StringContent(requestBody, Encoding.UTF8, "application/json"),
        };
    }

    private async Task<TResponse> SendAsync<TResponse>(Func<Task<HttpRequestMessage>> httpRequestFactory, CancellationToken cancellationToken)
    {
        using var httpResponse = await SendHttpRequestAsync(httpRequestFactory, cancellationToken);

        if (!httpResponse.IsSuccessStatusCode)
        {
            var errorContent = await httpResponse.Content.ReadAsStringAsync(cancellationToken);
            throw new CrmApiCommunicationException(httpResponse.StatusCode, errorContent: errorContent);
        }

        var responseContent = await httpResponse.Content.ReadAsStringAsync(cancellationToken);
        if (string.IsNullOrEmpty(responseContent))
        {
            throw new CrmApiSerializationException(responseContent: responseContent);
        }

        try
        {
            return JsonSerializer.Deserialize<TResponse>(responseContent, _jsonSerializerOptions) ??
                   throw new CrmApiSerializationException(responseContent: responseContent);
        }
        catch (Exception ex) when (ex is JsonException or NotSupportedException or ArgumentNullException)
        {
            throw new CrmApiSerializationException(ex, responseContent);
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
