namespace HE.Investments.Api.Config;

public interface IApiConfig
{
    string? BaseUri { get; }

    IDictionary<string, string> Apis { get; }

    int RetryCount { get; }

    int RetryDelayInMilliseconds { get; }

    string GetApiUrl(string apiName);
}
