using HE.Investments.Api.Config;
using Microsoft.Extensions.DependencyInjection;

namespace HE.Investments.Api.Extensions;

public static class HttpClientBuilderExtensions
{
    public static void ConfigureApiHttpClient(this IHttpClientBuilder builder)
    {
        builder.ConfigureHttpClient((services, client) =>
        {
            var config = services.GetRequiredService<IApiConfig>();
            if (!string.IsNullOrEmpty(config.BaseUri))
            {
                client.BaseAddress = new Uri(config.BaseUri);
            }
        });
    }
}
