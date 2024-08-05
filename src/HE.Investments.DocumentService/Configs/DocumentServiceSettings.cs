using Microsoft.Extensions.Configuration;

namespace HE.Investments.DocumentService.Configs;

public class DocumentServiceSettings : IDocumentServiceSettings
{
    public DocumentServiceSettings(IConfiguration configuration)
    {
        Url = configuration.GetValue<string>("AppConfiguration:UtilsService:Url") ?? string.Empty;
        UseMock = configuration.GetValue<bool>("AppConfiguration:UtilsService:UseMock");
        if (!UseMock && string.IsNullOrWhiteSpace(Url))
        {
            throw new InvalidOperationException("Missing required Document Service Url.");
        }
    }

    public string Url { get; }

    public bool UseMock { get; }
}
