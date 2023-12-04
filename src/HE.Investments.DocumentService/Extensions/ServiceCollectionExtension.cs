using System.Net;
using HE.Investments.DocumentService.Configs;
using HE.Investments.DocumentService.Services;
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
        services.AddSingleton<IDocumentServiceSettings, DocumentServiceSettings>();
        services.AddSingleton<HttpDocumentService>();
        services.AddSingleton<MockedDocumentService>();

        services.AddScoped(GetDocumentService);
    }

    private static bool ShouldRetry(HttpResponseMessage response)
    {
        return response.StatusCode >= HttpStatusCode.InternalServerError;
    }

    private static IDocumentService GetDocumentService(IServiceProvider serviceProvider)
    {
        var settings = serviceProvider.GetRequiredService<IDocumentServiceSettings>();
        if (settings.UseMock)
        {
            return serviceProvider.GetRequiredService<MockedDocumentService>();
        }

        return serviceProvider.GetRequiredService<HttpDocumentService>();
    }
}
