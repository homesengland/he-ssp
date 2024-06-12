namespace HE.Investments.Api.Config;

public interface IApiConfig
{
    string? BaseUri { get; }

    string? SubscriptionKey { get; }

    int RetryCount { get; }

    int RetryDelayInMilliseconds { get; }
}
