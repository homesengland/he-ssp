namespace HE.Investments.Api.Config;

internal sealed class ApiConfig : IApiConfig
{
    public string? BaseUri { get; set; }

    public string? SubscriptionKey { get; set; }

    public int RetryCount { get; set; } = 1;

    public int RetryDelayInMilliseconds { get; set; } = 250;
}
