using Microsoft.Extensions.Configuration;

namespace HE.Investments.DocumentService.Configs;

public class UtilsServiceSettings : IUtilsServiceSettings
{
    public UtilsServiceSettings(IConfiguration configuration)
    {
        Url = configuration.GetValue<string>("AppConfiguration:UtilsService:Url") ?? string.Empty;
        UseMock = configuration.GetValue<bool>("AppConfiguration:UtilsService:UseMock");
        if (!UseMock && string.IsNullOrWhiteSpace(Url))
        {
            throw new InvalidOperationException("Missing required Utils Service Url.");
        }
    }

    public string Url { get; }

    public bool UseMock { get; }
}
