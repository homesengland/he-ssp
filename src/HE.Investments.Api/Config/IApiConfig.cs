namespace HE.Investments.Api.Config;

public interface IApiConfig
{
    string? BaseUri { get; }

    int RetryCount { get; }

    int RetryDelayInMilliseconds { get; }
}
