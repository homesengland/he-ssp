using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http.Json;
using RichardSzalay.MockHttp;

namespace HE.Investments.IntegrationTestsFramework;

public class IntegrationTestsHttpClientFactory : IHttpClientFactory
{
    private const string BaseUrl = "https://localhost/integration-tests-mock/";

    private static readonly Dictionary<string, HttpClient> MockedClients = [];

    private readonly IHttpClientFactory _decorated;

    public IntegrationTestsHttpClientFactory(IHttpClientFactory decorated)
    {
        _decorated = decorated;
    }

    [SuppressMessage("Reliability", "CA2000", Justification = "Http client ownership goes to DI container.")]
    public static void RegisterMockedClient(string httpClientName, params (HttpMethod HttpMethod, string Url, object? Response)[] httpClientSetups)
    {
        var httpClientMock = new MockHttpMessageHandler();
        foreach (var (httpMethod, url, response) in httpClientSetups)
        {
            httpClientMock.When(httpMethod, $"{BaseUrl}{url}")
                .Respond(_ => new HttpResponseMessage(HttpStatusCode.OK) { Content = JsonContent.Create(response) });
        }

        var httpClient = httpClientMock.ToHttpClient();
        httpClient.BaseAddress = new Uri(BaseUrl);

        MockedClients[httpClientName] = httpClient;
    }

    public HttpClient CreateClient(string name)
    {
        if (MockedClients.TryGetValue(name, out var httpClient))
        {
            return httpClient;
        }

        return _decorated.CreateClient(name);
    }
}
