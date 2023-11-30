using System.Net;
using HE.Investments.DocumentService.Configs;
using HE.Investments.DocumentService.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;

namespace HE.Investments.DocumentService.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddDocumentServiceModule(this IServiceCollection services)
    {
        var retryPolicyNeedsTrueResponse = Policy.HandleResult<HttpResponseMessage>(ShouldRetry)
            .WaitAndRetryAsync(
                1,
                _ => TimeSpan.FromSeconds(10),
                onRetry: (_, _, _, _) => { });

        services.AddHttpClient("HE.Investments.DocumentService").AddPolicyHandler(retryPolicyNeedsTrueResponse);
        services.AddSingleton<IDocumentService, HttpDocumentService>();

        services.AddSingleton<IDocumentServiceConfig>(x =>
            x.GetRequiredService<IConfiguration>().GetRequiredSection("AppConfiguration:DocumentService").Get<DocumentServiceConfig>());
    }

    private static bool ShouldRetry(HttpResponseMessage response)
    {
        return response.StatusCode >= HttpStatusCode.InternalServerError;
    }
}
