namespace HE.Investments.Common.Models.App;

public interface ICrmApiConfig
{
    string? BaseUri { get; }

    string? SubscriptionKey { get; }

    int RetryCount { get; }

    int RetryDelayInMilliseconds { get; }
}
