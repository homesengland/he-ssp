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
#pragma warning disable IDE0078 // Use pattern matching
        var retryPolicyNeedsTrueResponse =
            Policy.HandleResult<HttpResponseMessage>(ex => ex.StatusCode != HttpStatusCode.OK && (ex.StatusCode != HttpStatusCode.Forbidden || ex.StatusCode != HttpStatusCode.Unauthorized))
                .WaitAndRetryAsync(
                    3,
                    sleepDurationProvider => TimeSpan.FromSeconds(10),
                    onRetry: (response, sleepDuration, attempt, context) => { }
                );
#pragma warning restore IDE0078 // Use pattern matching
        services.AddHttpClient("HE.Investments.DocumentService").AddPolicyHandler(retryPolicyNeedsTrueResponse);
        services.AddSingleton<IHttpDocumentService, HttpDocumentService>();

        services.AddSingleton<IDocumentServiceConfig>(x =>
            x.GetRequiredService<IConfiguration>().GetRequiredSection("AppConfiguration:DocumentService").Get<DocumentServiceConfig>());
    }
}
