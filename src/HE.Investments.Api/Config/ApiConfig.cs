namespace HE.Investments.Api.Config;

internal sealed class ApiConfig : IApiConfig
{
    public string? BaseUri { get; set; }

    public IDictionary<string, string> Apis { get; set; }

    public int RetryCount { get; set; } = 1;

    public int RetryDelayInMilliseconds { get; set; } = 250;

    public string GetApiUrl(string apiName)
    {
        return Apis.TryGetValue(apiName, out var apiUrl) ? apiUrl : string.Empty;
    }
}
