using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http.Json;
using Moq;
using RichardSzalay.MockHttp;

namespace HE.Investments.IntegrationTestsFramework;

public static class MockedHttpClient
{
    private const string BaseUrl = "https://localhost/integration-tests-mock/";

    private static readonly Mock<IHttpClientFactory> FactoryMock = new();

    public static IHttpClientFactory Factory => FactoryMock.Object;

    [SuppressMessage("Reliability", "CA2000", Justification = "Http client ownership goes to DI container.")]
    public static void RegisterClient(string httpClientName, params (HttpMethod HttpMethod, string Url, object Response)[] httpClientSetups)
    {
        var httpClient = new MockHttpMessageHandler();
        foreach (var (httpMethod, url, response) in httpClientSetups)
        {
            httpClient.When(httpMethod, $"{BaseUrl}{url}")
                .Respond(_ => new HttpResponseMessage(HttpStatusCode.OK) { Content = JsonContent.Create(response) });
        }

        FactoryMock.Setup(x => x.CreateClient(httpClientName)).Returns(() =>
        {
            var client = httpClient.ToHttpClient();
            client.BaseAddress = new Uri(BaseUrl);

            return client;
        });
    }
}
