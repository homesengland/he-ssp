using System.Net;
using HE.Investments.DocumentService.Configs;
using HE.Investments.DocumentService.Services;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Microsoft.Extensions.Configuration;

namespace HE.Investments.DocumentService.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddDocumentServiceModule(this IServiceCollection services)
    {
        var retryPolicyNeedsTrueResponse =
            Policy.HandleResult<HttpResponseMessage>(ex => ex.StatusCode != HttpStatusCode.OK && (ex.StatusCode != HttpStatusCode.Forbidden || ex.StatusCode != HttpStatusCode.Unauthorized))
                .WaitAndRetryAsync(
                    3,
                    _ => TimeSpan.FromSeconds(10),
                    onRetry: (_, _, _, _) => { }
                );

        services.AddHttpClient("HE.Investments.DocumentService").AddPolicyHandler(retryPolicyNeedsTrueResponse);
        services.AddSingleton<IHttpDocumentService, HttpDocumentService>();

        services.AddSingleton<IDocumentServiceConfig>(x =>
            x.GetRequiredService<IConfiguration>().GetRequiredSection("AppConfiguration:DocumentService").Get<DocumentServiceConfig>());
    }
}
